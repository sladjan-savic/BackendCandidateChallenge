INSERT INTO [Quiz] (Title) VALUES ('My first quiz');
INSERT INTO [Question] (Text, QuizId) VALUES ('My first question', 1);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My first answer to first q', 1);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My second answer to first q', 1);
UPDATE [Question] SET CorrectAnswerId = 1 WHERE Id = 1;
INSERT INTO [Question] (Text, QuizId) VALUES ('My second question', 1);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My first answer to second q', 2);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My second answer to second q', 2);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My third answer to second q', 2);
UPDATE [Question] SET CorrectAnswerId = 5 WHERE Id = 2;

INSERT INTO [Quiz] (Title) VALUES ('My second quiz');

INSERT INTO [Quiz] (Title) VALUES ('My third quiz');
INSERT INTO [Question] (Text, QuizId) VALUES ('Q1', 3);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My first answer to first q', 3);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My second answer to first q', 3);
UPDATE [Question] SET CorrectAnswerId = 5 WHERE Id = 3;
INSERT INTO [Question] (Text, QuizId) VALUES ('Q2', 3);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My first answer to second q', 4);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My second answer to second q', 4);
INSERT INTO [Answer] (Text, QuestionId) VALUES ('My third answer to second q', 4);
UPDATE [Question] SET CorrectAnswerId = 9 WHERE Id = 4;
