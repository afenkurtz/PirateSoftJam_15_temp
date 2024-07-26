EXTERNAL playEmote(emoteName)

~ playEmote("exclamation")

Hello there!
-> main

=== main === 
How are you feeling today?
+ [Happy]
    That makes me feel <color=\#F8FF30>happy</color> as well!
+ [Sad]
    Oh, well that makes me <color=\#FF1E35>sad</color> too.
    
- Dont trust him, hes not a real doctor!

Well, do you have any more questions?
+ [Yes]
    -> main
+ [No]
    Goodbye then!
    -> END