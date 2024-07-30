INCLUDE globals.ink

EXTERNAL updateMoney(player_money)

Hello there!
-> main

=== main ===
Want a moneys?
    + [Yes]
        -> chosen(5)
    + [No]
        -> chosen(0)
   
        
=== chosen(value) ===
~ player_money += value
okay. you have {player_money} muney
~ updateMoney(player_money)
-> END