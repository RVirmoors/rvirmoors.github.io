
using Tickblaze.Scripts.Indicators;
using Tickblaze.Scripts.Api.Interfaces.Orders;
// using MyScripts.Indicators;

using System.IO;
namespace MyScripts.Strategies;

public partial class RangeDays : Indicator
{
    [Parameter("Period"), NumericRange(1, int.MaxValue)]
    public int Period { get; set; } = 5;

    [Plot("Result")]
	public PlotSeries Result { get; set; } = new(Color.Blue);

    public RangeDays()
    {
        Name = "Range over the Past Days";
        ShortName = "rangeD";
        IsOverlay = true;
    }

    protected override void Calculate(int index)
    {
        if (index < Period)
            return; // Not enough data to calculate

        var high = Bars.High[index];
        var low = Bars.Low[index];

        for (int i = 1; i <= Period; i++)
        {
            if (index - i < 0)
                break; // Prevent out of bounds access
            if (Bars.High[index - i] > high)
                high = Bars.High[index - i];
            if (Bars.Low[index - i] < low)
                low = Bars.Low[index - i];
        }
        Result[index] = high - low;
    }
}

public partial class ConsolidationDays : Indicator
{
    [Parameter("Maximum Range multiples of ATR"), NumericRange(0.01, 10)]
    public double MaxRangeTimesATR { get; set; } = 2.0;

    [Plot("Result")]
	public PlotSeries Result { get; set; } = new(Color.Blue);
    private AverageTrueRange? _atrIndicator;

    public ConsolidationDays()
    {
        Name = "Consolidation Days";
        ShortName = "Consolidation";
        IsOverlay = true;
        _atrIndicator = new AverageTrueRange(14, MovingAverageType.WellesWilder);
    }

    protected override void Calculate(int index)
    {
        if (index < 2)
            return; // Not enough data to calculate

        double atr = _atrIndicator![index];
        double maxRange = MaxRangeTimesATR * atr;
        int count = 0;
        double maxPrice = Bars.High[index];
        double minPrice = Bars.Low[index];
        int i = index - 1;
        while (i >= 0 && (maxPrice - minPrice) <= maxRange)
        {
            if (Bars.High[i] > maxPrice)
                maxPrice = Bars.High[i];
            if (Bars.Low[i] < minPrice)
                minPrice = Bars.Low[i];
            count++;
            i--;
        }
        Result[index] = count;
    }
}

public partial class RelativeVolume : Indicator
{
    [Parameter("Period"), NumericRange(1, int.MaxValue)]
    public int Period { get; set; } = 20;

    [Plot("Result")]
    public PlotSeries Result { get; set; } = new(Color.Blue);

    public RelativeVolume()
    {
        Name = "Relative Volume";
        ShortName = "RelVol";
        IsOverlay = true;
    }

    protected override void Calculate(int index)
    {
        if (index < Period)
            return; // Not enough data to calculate

        double avgVolume = 0;
        for (int i = 0; i < Period; i++)
        {
            if (index - i < 0)
                break; // Prevent out of bounds access
            avgVolume += Bars.Volume[index - i];
        }
        avgVolume /= Period;

        Result[index] = Bars.Volume[index] / avgVolume;
    }
}

public partial class ADR : Indicator
{
    [Parameter("Period"), NumericRange(1, int.MaxValue)]
    public int Period { get; set; } = 20;

    [Plot("Result")]
    public PlotSeries Result { get; set; } = new(Color.Blue);

    public ADR()
    {
        Name = "Average Daily Range %";
        ShortName = "ADR%";
        IsOverlay = true;
    }

    protected override void Calculate(int index)
    {
        double sum = 0;
        for (int i = 0; i < Period; i++)
        {
            if (index - i < 0)
                break; // Prevent out of bounds access
            sum += Bars.High[index - i] / Bars.Low[index - i];
        }
        Result[index] = (sum / Period - 1) * 100; // Return as percentage
    }
}

public partial class HigherLows : Indicator
{
    [Parameter("Range"), NumericRange(1, int.MaxValue)]
    public int Range { get; set; } = 1;

    [Parameter("Lookback Period"), NumericRange(1, int.MaxValue)]
    public int LookbackPeriod { get; set; } = 30;

    [Plot("Result")]
    public PlotSeries Result { get; set; } = new(Color.Blue);
    public HigherLows()
    {
        Name = "Higher Lows";
        ShortName = "HL";
        IsOverlay = true;
    }

    private bool IsPivotLow(int index, int range = 1)
    {
        // Check if the current low is lower than the previous and next lows within the specified range
        for (int i = 1; i <= range; i++)
        {
            if (index - i < 0 || index + i >= Bars.Count)
                return false; // Prevent out of bounds access
            if (Bars.Low[index] >= Bars.Low[index - i] || Bars.Low[index] >= Bars.Low[index + i])
                return false; // Not a pivot low
        }
        return true; // It's a pivot low
    }

    protected override void Calculate(int index)
    {
        if (index < Range)
            return; // Not enough data to calculate

        // in the Lookback period, count how many pivot lows are higher than the previous pivot low

        // first, make a list of pivot lows
        var pivotLows = new List<int>();
        for (int i = index - LookbackPeriod; i <= index; i++)
        {
            if (i < 0)
                continue; // Prevent out of bounds access
            if (IsPivotLow(i, Range))
                pivotLows.Add(i);
        }
        if (pivotLows.Count < 2)
        {
            Result[index] = 0; // Not enough pivot lows to determine higher lows
            return;
        }
        // now, look backwards through the pivot lows and check if they are higher than the previous one
        int higherLowsCount = 0;
        for (int i = pivotLows.Count - 1; i > 0; i--)
        {
            if (Bars.Low[pivotLows[i]] <= Bars.Low[pivotLows[i - 1]])
            {
                break; // Stop counting if we find a low that is not higher than the previous one
            }
            higherLowsCount++;
        }
        Result[index] = higherLowsCount; // Store the count of higher lows
    }
}

// =========================================================

public class TightBO : Strategy
{
    [Parameter("30 Day Performance Minimum"), NumericRange(0.01, 100)]
    public double PerformanceMinimum { get; set; } = 50.0;

    [Parameter("ADR Minimum"), NumericRange(0.01, 100)]
    public double AdrMinimum { get; set; } = 5.0;

    [Parameter("Dollar Volume Minimum"), NumericRange(100000, double.MaxValue)]
    public double DollarVolumeMinimum { get; set; } = 200000.0;

    [Parameter("Higher Lows Minimum"), NumericRange(0, double.MaxValue)]
    public double HigherLowsMininum { get; set; } = 0.0;

    [Parameter("Max No. of Candidates"), NumericRange(1, int.MaxValue)]
    public int NumberOfCandidates { get; set; } = 10;

    [Parameter("Percent of Candidates"), NumericRange(0.01, 1.0)]
    public double PercentOfCandidates { get; set; } = 0.125; // 12.5%

    [Parameter("Annual Risk Target"), NumericRange(0.01, 0.75)]
    public double AnnualRiskTarget { get; set; } = 0.1; // 10% of account equity

    [Parameter("Stop Loss ATR Multiplier"), NumericRange(0.1, 10)]
    public double StopLossAtrMultiplier { get; set; } = 1.5;
    [Parameter("EMA exit period"), NumericRange(1, int.MaxValue)]
    public int EmaExitPeriod { get; set; } = 20;

    [Parameter("Breakeven after N days"), NumericRange(1, 30)]
    public int BreakevenDays { get; set; } = 5;

    private Performance? _performanceIndicator;
    private int PerformancePeriod => 30;
    private Performance? _performanceIndicator6Months;
    private ADR? _adrIndicator;
    private AverageTrueRange? _atrIndicator;
    private RangeDays? _rangeDaysIndicatorFive;
    private RangeDays? _rangeDaysIndicatorTen;
    private MovingAverage? _ema20Indicator;
    private MovingAverage? _sma200Indicator;
    private MovingAverage? _emaExitIndicator;
    private HigherLows? _higherLowsIndicatorOne;
    private HigherLows? _higherLowsIndicatorTwo;
    private ConsolidationDays? _consolidationDaysIndicator;
    private RelativeVolume? _relativeVolumeIndicator;
    private string filePath = "performance_results.txt";
    private string candidatesFilePath = "candidates.csv";

    private static Dictionary<string, double> scanResults = new Dictionary<string, double>();
    private static Dictionary<string, double> candidates = new Dictionary<string, double>();
    private static DateTime prevDay = DateTime.MinValue;
    private IOrder? entry;
    private int entryBar;

    protected override void Initialize()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
        if (File.Exists(candidatesFilePath))
            File.Delete(candidatesFilePath);

        _performanceIndicator = new Performance(Bars.Close, PerformancePeriod);
        _performanceIndicator6Months = new Performance(Bars.Close, 128);
        _adrIndicator = new ADR(20);
        _atrIndicator = new AverageTrueRange(14, MovingAverageType.WellesWilder);
        _rangeDaysIndicatorFive = new RangeDays { Period = 5 };
        _rangeDaysIndicatorTen = new RangeDays { Period = 10 };
        _consolidationDaysIndicator = new ConsolidationDays { MaxRangeTimesATR = 3.0 };
        _relativeVolumeIndicator = new RelativeVolume { Period = 10 };
        _ema20Indicator = new MovingAverage(Bars.Close, 20, MovingAverageType.Exponential);
        _sma200Indicator = new MovingAverage(Bars.Close, 200, MovingAverageType.Simple);
        _higherLowsIndicatorOne = new HigherLows { Range = 1, LookbackPeriod = 30 };
        _higherLowsIndicatorTwo = new HigherLows { Range = 2, LookbackPeriod = 45 };
        _emaExitIndicator = new MovingAverage(Bars.Close, EmaExitPeriod, MovingAverageType.Exponential);

        if (!File.Exists(candidatesFilePath))
        {
            string candidatesHeader = "Date,Symbol,RelVolume,DollarVolume,30Dperf,6Mperf,ADR%,$Range5D,$Range10D,Close,EMA20,ATR,HLows\n";
            File.AppendAllText(candidatesFilePath, candidatesHeader);
        }
    }

    protected override void OnBar(int index)
    {
        if (index <= PerformancePeriod)
            return; // Not enough data to calculate performance

        DateTime currentDay = Bars[index].Time;
        if (currentDay != prevDay)
        {
            if (scanResults.Count > 0)
            {
                // sort scanResults by value in descending order
                scanResults = scanResults.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                int numCandidates = Math.Min(NumberOfCandidates, (int)(scanResults.Count * PercentOfCandidates));
                candidates = scanResults
                    .Take(numCandidates)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                string c = string.Join(", ", candidates.Keys);
                File.AppendAllText(filePath, $"{Bars[index].Time} ==== {numCandidates}/{scanResults.Count} Candidates:\n{c}\n\n");
                scanResults.Clear();
            }
            else
            {
                candidates.Clear();
                File.AppendAllText(filePath, $"{Bars[index].Time} ==== No candidates found.\n");
            }
        }

        var performanceValue = _performanceIndicator![index];
        var performance6MonthsValue = _performanceIndicator6Months![index] != 0 ? _performanceIndicator6Months![index] : performanceValue * 1.666;
        var adrValue = _adrIndicator![index];
        var symbolCode = Bars.Symbol.Code;
        double dollarVolume = Bars.Volume[index] * Bars.Close[index];
        double relVolume = _relativeVolumeIndicator![index];
        var rangeFiveMax = 2 * _atrIndicator![index]; // 2 * adrValue / 100 * Bars.Close[index];
        var rangeTenMax = 4 * _atrIndicator![index];// 4 * adrValue / 100 * Bars.Close[index];
        var closeEmaMin = _ema20Indicator![index] - 0.25 * _atrIndicator![index];
        var closeEmaMax = _ema20Indicator![index] + 1.75 * _atrIndicator![index];
        var higherLows = _higherLowsIndicatorOne![index] + _higherLowsIndicatorTwo![index] * 0.5;

        // TRADE MANAGEMENT
        if (Position != null)
        {
            // Close position if price is below MA
            var ma_price = _emaExitIndicator![index];
            if (Bars.Close[index] < ma_price)
            {
                ClosePosition();
                entry = null;
                entryBar = 0;
                File.AppendAllText(filePath, $"{Bars[index].Time} | {symbolCode} EXIT LONG at {Bars.Close[index]:F2} (below MA)\n");
                return;
            }
            int numberOfBarsSinceEntry = Bars.Count - entryBar;
            // if entry was 4 days ago, move stop loss to breakeven
            if (entry != null && numberOfBarsSinceEntry == BreakevenDays)
            {
                double breakeven = Bars.Close[index - BreakevenDays] * 1.03;

                entry = SetStopLoss(entry, breakeven);
                File.AppendAllText(filePath, $"{Bars[index].Time} | {symbolCode} MOVE STOPLOSS to breakeven at {breakeven:F2}\n");
            }
        }

        // ENTER TRADES
        // if the stock name is in candidates, then enter a trade
        if (candidates.ContainsKey(symbolCode) && Position == null)
        {
            double exposure = 100 * AnnualRiskTarget / 15.87 * Account.NetLiquidation / adrValue;
            int quantity = (int)(exposure / Bars.Close[index]);
            if (quantity > 0)
            {
                double sl = Bars.Close[index] - _atrIndicator![index] * StopLossAtrMultiplier;
                sl = Math.Min(sl, Bars.Low[index]);

                // get maximum close over last 8 bars
                double maxClose = 0;
                for (int i = 1; i <= 8; i++)
                {
                    if (index - i < 0)
                        break; // Prevent out of bounds access
                    if (Bars.Close[index - i] > maxClose)
                        maxClose = Bars.Close[index - i];
                }
                if (Bars.Close[index] > maxClose)
                {
                    // (tickblaze will enter on next day's open. In my case I execute on the close of the current bar)
                    entry = ExecuteMarketOrder(OrderAction.Buy, quantity);
                    entry = SetStopLoss(entry, sl);
                    entryBar = Bars.Count;
                    File.AppendAllText(filePath, $"{Bars[index].Time} | {symbolCode} ENTER LONG at {Bars.Close[index]:F2}, STOPLOSS at {sl:F2}\n");
                }
            }
        }

        prevDay = currentDay;
        // SCAN FOR NEXT DAY
        if (symbolCode == "RDDT" || symbolCode == "SOUN" || symbolCode == "RGTI" || symbolCode == "TSSI")
            // if (Bars[index].Time.Date == new DateTime(2024, 11, 21))
            File.AppendAllText(filePath, $"{Bars[index].Time}. {symbolCode} | 30Dperf {performanceValue:F2}, 6Mperf {performance6MonthsValue:F2}, ATR {_atrIndicator![index]:F2}, ADR {adrValue:F2}%, $volume: {dollarVolume:F0}, Consld: {_consolidationDaysIndicator[index]:F0}, Range: {_rangeDaysIndicatorFive[index]:F2} < {rangeFiveMax:F2}, {_rangeDaysIndicatorTen[index]:F2} < {rangeTenMax:F2}, Close {Bars.Close[index]:F2} in ({closeEmaMin:F2}, {closeEmaMax:F2}), HL: {higherLows}\n");

        if (performanceValue < PerformanceMinimum ||
            // performance6MonthsValue < PerformanceMinimum * 1.666 ||
            adrValue < AdrMinimum ||
            dollarVolume < DollarVolumeMinimum ||
            _rangeDaysIndicatorFive![index] > rangeFiveMax ||
            _rangeDaysIndicatorTen![index] > rangeTenMax ||
            // _consolidationDaysIndicator![index] < 6 ||
            Bars.Close[index] < closeEmaMin ||
            Bars.Close[index] > closeEmaMax ||
            _ema20Indicator![index] < _sma200Indicator![index] ||
            higherLows < HigherLowsMininum
            )
            return;
        scanResults[symbolCode] = dollarVolume;
        // write candidates to file
        string candidateLine = $"{Bars[index].Time.Date:yyyy-MM-dd},{symbolCode},{relVolume:F0},{dollarVolume:F0},{performanceValue:F2},{performance6MonthsValue:F2},{adrValue:F2},{_rangeDaysIndicatorFive[index]:F2},{_rangeDaysIndicatorTen[index]:F2},{Bars.Close[index]:F2},{_ema20Indicator![index]:F2},{_atrIndicator![index]:F2},{higherLows}\n";
        File.AppendAllText(candidatesFilePath, candidateLine);
    }
}
