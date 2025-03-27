EXTERNAL start_quest()
EXTERNAL finish_quest()
VAR quest_state = 0

{quest_state:
- 0: -> music_already_playing
- 1: -> quest_not_start
- 2: -> quest_is_in_progress
- 3: -> quest_can_finished
- 4: -> quest_finished
}

=== music_already_playing ===
#speaker:Крутой курильщик #layout:text_name_right
Хорошо отдыхаю

-> END

=== quest_not_start ===
#speaker:Крутой курильщик #layout:text_name_right
Что-то тоскливо как-то
Никак нет настроения...

#speaker:Трактирщик #layout:text_name_left
Может я развеселю вас песней?!

#speaker:Крутой курильщик #layout:text_name_right
Сомневаюсь, конечно...
~ start_quest()
Но попробовать можно

-> END

=== quest_is_in_progress ===
#speaker:Крутой курильщик #layout:text_name_right
Эх, скука...
-> END

=== quest_can_finished ===
#speaker:Крутой курильщик #layout:text_name_right
ОООО-О-О
Классная песня!

#speaker:Трактирщик #layout:text_name_left
~ finish_quest()
Да, мне тоже нравится
-> END

=== quest_finished ===
#speaker:Крутой курильщик #layout:text_name_right
Панки, хой!
-> END