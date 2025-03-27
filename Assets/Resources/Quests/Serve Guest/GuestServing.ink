EXTERNAL start_quest()
EXTERNAL finish_quest()
EXTERNAL serve_guest()

VAR can_serve = false
VAR quest_state = 0

{quest_state:
- 0: 
- 1: -> quest_not_start
- 2: -> wait_for_beer
- 3:
- 4: -> beer
}

=== quest_not_start ===
#speaker:Усатый Скала #layout:text_name_right
Эх, пивка бы сейчас

#speaker:Трактирщик #layout:text_name_left
~ start_quest()
Я вас услышал!

-> END

=== wait_for_beer ===
#speaker:Усатый Скала #layout:text_name_right
Ну что, где моё пиво? 

{can_serve == true:
    -> can_serve_dialogue
 -else:
    -> cant_serve_dialogue
}

=== can_serve_dialogue ===
+ [*подать пиво*]
    #speaker:Трактирщик #layout:text_name_left
    ~ serve_guest()
    Вот ваше пиво!
    #speaker:Усатый Скала #layout:text_name_right
    Ну и ссанина конечно
    Эх. Раз уж отпил, на свои деньги
    -> END
+ [Ещё не готово]
    #speaker:Трактирщик #layout:text_name_left
    Ваш заказ задерживается
    -> END

=== cant_serve_dialogue ===
#speaker:Трактирщик #layout:text_name_left
Ваш заказ задерживается
-> END

=== beer ===
#speaker:Усатый Скала #layout:text_name_right
Больше никогда сюда не зайду
-> END
