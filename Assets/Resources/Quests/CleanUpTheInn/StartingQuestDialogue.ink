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
Ну и срач тут у тебя
Даже мне, коту, не приятно

* [Уберу]
    #speaker:Трактирщик #layout:text_name_left
    ~ start_quest()
    Мы же ещё даже не открылись
    Сейчас всё приберу
    -> END
    
* [Лень убирать]
    #speaker:Трактирщик #layout:text_name_left
    Мне лень тут убираться
    
    #speaker:Кот #layout:text_name_right
    Ну ты и балбес конечно
    Даже не представляю как долг дьяволу выплачивать будешь
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
Я же говорил, что всё уберу

#speaker:Кот #layout:text_name_right
О-о-ой, хвалю хвалю!
~ finish_quest()
На вот горстку золотых и иди гостей встречай
А я тут своими дьявольскими делами занят
-> END

=== quest_finished ===
#speaker:Кот #layout:text_name_right
Иди гостей обслуживай!
А то будут говорить ещё: "Во трактирщик дурак! С котом разговаривает!"
-> END