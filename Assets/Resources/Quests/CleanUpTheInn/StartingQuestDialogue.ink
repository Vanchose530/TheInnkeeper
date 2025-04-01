EXTERNAL start_quest()
EXTERNAL finish_quest()
VAR quest_state = 0

{quest_state:
- 0: -> inn_already_clean
- 1: -> quest_not_start
- 2: -> quest_is_in_progress
- 3: -> quest_can_finished
- 4: -> quest_finished
}
=== inn_already_clean ===
#speaker:Кот #layout:text_name_right
Не лезь ко мне
Видишь, я занят?

-> END

=== quest_not_start ===
#speaker:Кот #layout:text_name_right
Мявк
Ну и бардак тут у тебя
Даже мне, коту, не приятно

* [Уберу]
    #speaker:Трактирщик #layout:text_name_left
    ~ start_quest()
    Мы же ещё даже не открылись
    Сейчас всё приберу
    -> END
    
* [Мне не до этого]
    #speaker:Трактирщик #layout:text_name_left
    Мне сейчас не до уборки
    
    #speaker:Кот #layout:text_name_right
    Ну ты и бездарный ты трактирщик конечно!
    Даже не представляю на какие шиши ты долг выплачивать будешь
    -> END

=== quest_is_in_progress ===
#speaker:Кот #layout:text_name_right
Долго ты ещё с этими лавочками возиться будешь?
Чтобы открыть трактир должна быть чистота!
Как сюда будут души... КХМ! Люди приходить?!
-> END

=== quest_can_finished ===
#speaker:Трактирщик #layout:text_name_left
Ну вот и всё!
Уборка закончена

#speaker:Кот #layout:text_name_right
Хвалю, хвалю
~ finish_quest()
Держи вот тебе горстку золотых да иди гостей встречай
А я тут пока своими дъявольскими делами займусь
-> END

=== quest_finished ===
#speaker:Кот #layout:text_name_right
Гости не поймут, если ты будешь при них с котом разговаривать
Так что марш работать
-> END