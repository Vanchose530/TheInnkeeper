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
#speaker:Вечный Курильщик #layout:text_name_right
Чаю пожалуста

#speaker:Трактирщик #layout:text_name_left
~ start_quest()
Скоро будет

-> END

=== wait_for_beer ===
#speaker:Вечный Курильщик #layout:text_name_right
Ну что, чай готов?

{can_serve == true:
    -> can_serve_dialogue
 -else:
    -> cant_serve_dialogue
}

=== can_serve_dialogue ===
+ [*подать чай*]
    #speaker:Трактирщик #layout:text_name_left
    ~ serve_guest()
    Вот ваш чай!
    #speaker:Вечный Курильщик #layout:text_name_right
    Спасибо, вот ваши деньги
    -> END
+ [Ещё не готов]
    #speaker:Трактирщик #layout:text_name_left
    Ваш чай ещё заваривается
    -> END

=== cant_serve_dialogue ===
#speaker:Трактирщик #layout:text_name_left
Ваш чай ещё заваривается
-> END

=== beer ===
#speaker:Вечный Курильщик #layout:text_name_right
Какой наваристый чай
-> END
