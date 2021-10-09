---
layout: post
title: Days of Future Past
---

I got into music through Metallica and Iron Maiden, and there's nothing you or I can do about it now. The past couple of weeks I made a fan edit of their new album -- here's <a href="#why">why</a>, <a href="#how">how</a>, <a href="#what">what</a>, and <a href="#where">where</a>.

## why
I started listening to Maiden in '99, just as Bruce and Adrian were rejoining the band, which turned out to be a great entry point. 21st century Maiden sounds very distinct from what came before, and beyond their massive following and Spinal Tap presentation there's some pretty idiosyncratic music.

My favourite album of this era is 2006's *A Matter of Life and Death*. At least I think it is... I haven't listened to since I lost the edit/remaster I made 15 (!) years ago. It disappeared in a hard drive failure and the only trace is on my [defunct previous blog](https://web.archive.org/web/20080509073411/http://prowler.wordpress.com/2006/09/06/my-matter-of-life-and-death/).[^1] (if you somehow have a copy, please get in touch!!!)

Their next album came in 2010, and I found it to be their worst ever. By then I was head-deep into my mind-expanding MMus experience, busy digging the exquisite sounds of Feldman and あざらし so there was no time to waste on sub-par Maiden. 2015's *Book of Souls* was good for a couple of spins, and not much more. I've been *over* Maiden, and most metal, for a good while now.

And yet, this month I've been indulging a dive back into the Irons thanks to their new one, *Senjutsu.* And since the album contains some of the weak traits of modern Maiden (which I'll go over <a href="#what">below</a>), I surrendered to the 15-year time loop and did a full-on fan edit.

## how
There's two tools I didn't have in 2006: source separation and spectral repair. As you'll see, these (almost) allow you to remix music that you don't have the stems to.

I used [demucs](https://github.com/facebookresearch/demucs) to split the songs into vocals-drums-bass-other tracks. There are currently two major music instrument separation libraries, accessible via Google Colab etc: [spleeter](https://github.com/deezer/spleeter) and demucs. I thought the latter produced cleaner results on this music, which allowed me to e.g. mute vocals in some parts, or do more seamless edits where everything doesn't jump from one section to another at the same time.

One of my biggest gripes with *Senjutsu* is the synth. It sounds harsh and (especially on the title track) it's played just terrible. Unfortunately, source separation isn't trained on synth pads, so most of it ended up in the "drums" and "other" tracks. There I was able to attenuate the high frequency components using Izotope RX. Here's what it looks like on the "other" track, on top of guitars.

![izotope rx spectral repair - other](/images/maiden/spectral-repair.png)

And here it is with drums. RX has a very transparent (to my ears) way of hiding certain components, as long as you select them by hand. Who knows, in 15 years maybe they'll have automated this too.

![izotope rx spectral repair - drums](/images/maiden/spectral-repair-drumz.png)

Now, there are limits to my patience so didn't do this everywhere, but I tried to get the worst offenders. You can still hear the synths, they're just a bit tamed now. I also used some other RX processing (de-click and de-clip to fix some artifacts from the excessively loud master, especially in 'Celts' and 'Parchment', until I got deathly bored, and EQ Match to ease some vocal transitions). And everything was put together in my current DAW of choice, Reaper.

OK, you must be dying for the track-by-track rundown, let's go:

## what

### 1. Senjutsu
This is *not* my favourite Maiden opener. Nothing too offensive (except for that synth, yuck), it plods along but I didn't dare cut too much or the flow would be ruined. Some uninspired instrumental bars did have to go, and so did the cheesy intro, replaced with a sudden transition from...

### 2. Stratego
Lyrically these first couple of songs are related, so I liked the idea to glue them into a continuous monster. I'm also very happy with the edit into the last chorus, removing a directionless breakdown. Bet you won't notice it. Anyway, this track is more energetic than the first one but still not that special. Happily, the album gets better...

### 3. The Writing on the Wall
This is as close to a conventionally killer song as we get here. Great choice for a first single, maybe Maiden's best since Wasted Years? I also love that it's a Maiden song that mentions riding but doesn't feature galloping guitars, rather it's more of a gangnam-style trot. Nothing to edit out of this song, it's perfectly constructed (and what a Smith solo!), but I did attenuate some of Nicko's off-beat hi-hats in the intro.

### 4. Lost in a Lost World
A Steve Harris epic with an unexpected beginning, which I loved. There are lyrical echoes all throughout this album, and this one seems to be connected to the closing track. I removed the first chorus and a couple of excessive repeats in the instrumental, to improve the flow. Also, I cut the mellow coda to a reasonable size by excising a kinda jingoistic verse towards the end.

### 5. Days of Future Past
Great verses and choruses but feels unfinished. Several tracks on the album seem to lose inspiration at some point and this one's the worst case of running out of ideas before the last chorus.

### 6. The Time Machine
Welp, another favourite. Signature Gers romper. The only problem I have with it, believe it or not, is that at 7+ minutes it's too damn short. It's presented as a sequence of stories in an immortal's life, and just as we're getting to the good stuff, the music veers into a Sign of the Cross'y interlude which however simply peters off. So, in probably my most questionable move, I grabbed the meat of the previous song (also about a time travellin' immortal) and stuffed it into this one. I tried several cuts and this one *just about* works... I hope.

### 7. Darkest Hour

Totally unnecessary track. So many better Smith-Dickinson power ballads exist, we don't need one here. This one goes in the bin, fuck Churchill.

### 8. Death of the Celts

This one I struggled with probably the most. First I was tempted to just write it off as derivative Harris war anthem drivel. There's more than one echo of *Virtual XI* here: the Clansman parallel is obvious, but the vocals build [into a verse](https://youtu.be/AsOFYvKohO4?t=208) that reminds me of nobody's favourite Maiden song, [The Educated Fool](https://youtu.be/LQXgNLGDPgo?t=98). But here's the thing: I love this kind of shit! It's played and sung so straight that it just has to pick you up. Unfortunately the instrumental is replete with aimless major scale run riffs that do nothing for me. Nicko's drumming, otherwise stellar on the album, also settles into a monotone shuffle, as if he's waiting for the noodling to end. I did my best to cut out the fat, because after all it's a good tune.

### 9. The Parchment

Another odd one, which takes a couple listens to reveal itself. I saw Bruce describe this one as hypnotic and he's right: there is a lot of repetition here but it actually works, and I wouldn't dare cut any of it out. The clean intro however is nothing special--if this were the only clean epic intro on the album (right!), things might be different. Unfortunately, like we've seen above, the song does reach a point where it runs out of ideas. Luckily that happens very close to the end, so I could just cut all of it. I'm not a fan of fadeouts, but in this case it's the best I could come up with.

### 10. Hell on Earth

I'll keep this one short. Awesome song, showcases everything good about the band, and particularly Harris's writing style.
I didn't touch this track, just a perfect Maiden closer.

### where

...can you get it? Right here ([mp3](https://mega.nz/file/ODIzmAha#xq3sVlIW1TBP9PdLD1JorimMw_zhETASUXcZptcyh_E)) and here ([flac](https://mega.nz/file/CGBxFKaT#E7gof8auYbCnldiW2gZAZNN22_MGQAHv8bBkFML16uM)).

Of course the tracklist is altered, and I also whipped up a style transfery cover. 

Here goes, enjoy...

![days of future past cover](/images/maiden/days-of-future-past.png)

1. Lost in a Lost World (7:32)
2. The Parchment (10:13)
3. The Writing on the Wall (6:13)
4. Stratego (4:31)
5. Senjutsu (5:31)
6. Death of the Celts (7:44)
7. Time Machine ... Days of Future Past (8:31)
8. <del>Hell on Earth (11:20)</del>* *(left unchanged, not included in the download)*

Running time: 62 minutes (down from 82).

[^1]: If you dig around that site you'll find some, hmm, problematic stuff. It was all meant in good dumb fun, but it's probably for the best that old man Wordpress cancelled it. 

