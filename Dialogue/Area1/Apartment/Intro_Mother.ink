INCLUDE ../../Globals.ink

Have a good night's sleep? You slept for quite some time.
+ [I was dreaming of Utopia again] -> utopia
+ [Dad said you had something for me?] -> gift

=== utopia ===
Well I dont know about Utopia, but your Dad and I wanted to help you get out and explore a little.
    + [Really? What do you mean?] -> gift
    
=== gift ===
We pulled together some of our savings and wanted to give you a chance to explore the mainland.
Genese can be beautiful this time of year, depending on the district of course.
* [Yeah well, Multos smells like shit, I'd bet any of the other districts would be better than here...] -> garbage
+ [Have you been out of Multos before?] -> outside
+ [So I can leave now?] -> leaving
-> END

=== outside ===
A few times, when your father and I were young.
Its expensive to travel, and when we had you we knew we needed to start saving everything we could.
+ [So I can leave now?] -> leaving

=== leaving ===
Well, we could only scrounge up enough for half of the gate fee. You'll have to do some work for people in the district to make up the rest.
Im sorry we couldnt do more.
+ [Thank you so much!] -> END
+ [Who can I ask about work?] -> work

=== work ===
I guess the Mayor would know if there's some work to be done.
You know where he lives, right? In the fancy house South-West of here.
-> END

=== garbage ===
Haha, well thats just because of the trash mountian. We're the only district with one of those.
-> gift