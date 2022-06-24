using Education.Application.Data.Repositories;
using Education.Domain.Announcement;
using Education.Domain.EduCourses;
using Education.Domain.EduModules;
using Education.Domain.EduTasks;
using Education.Domain.EduTests;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.SeedWork
{
    public class SeedData
    {
        public static async void EnsureDataCreated(IServiceProvider serviceProvider)
        {
            var _courseRepo = (ICourseRepository)serviceProvider.GetRequiredService(typeof(ICourseRepository));
            var _moduleRepo = (IModuleRepository)serviceProvider.GetRequiredService(typeof(IModuleRepository));
            var _annRepo = (IAnnouncementRepository)serviceProvider.GetRequiredService(typeof(IAnnouncementRepository));
            var _taskRepo = (IEduTaskRepository)serviceProvider.GetRequiredService(typeof(IEduTaskRepository));
            var _testRepo = (IEduTestRepository)serviceProvider.GetRequiredService(typeof(IEduTestRepository));

            var any = await _courseRepo.FindAsync(0, 1);
            if (any.Value.Count() == 0)
            {
                var coursedata = GetCourseSeedData();
                foreach (var item in coursedata)
                {
                    await _courseRepo.Create(item);
                }

                var announceCourseData = GetCourseAnnounceSeedData(coursedata);
                foreach (var item in announceCourseData)
                {
                    await _annRepo.Create(item);
                }

                var moduledata = GetModuleSeedData(coursedata);
                foreach (var item in moduledata)
                {
                    await _moduleRepo.Create(item);
                }

                var announceData = GetAnnounceSeedData(moduledata);
                foreach (var item in announceData)
                {
                    await _annRepo.Create(item);
                }

                var taskData = GetTaskSeedData(moduledata);
                foreach (var item in taskData)
                {
                    await _taskRepo.Create(item);
                }

                var testData = GetTestSeedData(moduledata);
                foreach (var item in testData)
                {
                    await _testRepo.Create(item);
                }
            }
        }


        public static IEnumerable<EduCourse> GetCourseSeedData()
        {
            var courses = new List<EduCourse>
            {
                new()
                {
                    Id = ObjectId.GenerateNewId(DateTime.Now),
                    Name = "Видеокурс построение сетей CISCO с нуля.",
                    Description = "Изучение сетевого оборудования Cisco, протоколов и механизмов посредством построения крупной корпоративной сети.",
                    Speciality = "Сети",
                    Students = new List<Guid>()
                  //  Users = new () {"f6240f24-6b77-43a3-b636-ff65a43995bd", "d9085df6-ce2b-49d2-9441-764c3d305732", "3445102c-8fc5-4be9-8e33-5bd229103f7d"}
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Сети с нуля. Просто - о сложном.",
                    Description = "Основы сетей: практический курс. Быстрый старт. Единственный Курс по Сетям на русском.",
                    Students = new List<Guid>()
                  //  Users = new (){"7b0d7fed-cfa1-44a0-b86b-4af6fa510935"}
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Полный курс Андроид + Java с нуля",
                    Description = "Изучаем с нуля язык Java и Android-разработку",
                    Students = new List<Guid>()
                  //  Users = new(){"a40b760b-46df-456c-9002-fc1d3beaea29"}
                },
                new()
                {     Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Тестировщик с нуля. Web, Mobile, Postman, SQL, Git, Bash",
                    Description = "Как стать тестировщиком с нуля? Профессия QA Engineer",
                    Students = new List<Guid>()
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Linux администратор. Лабораторные работы. Практический курс.",
                    Description = "Лабораторные работы. Практический курс.",
                    Students = new List<Guid>()
                  //  Users = new () {"f6240f24-6b77-43a3-b636-ff65a43995bd", "d9085df6-ce2b-49d2-9441-764c3d305732"}
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Fullstack приложение на ASP Net Core и React",
                    Description = "Создание полностью функционального веб-приложения при помощи C# и Javascript",
                    Students = new List<Guid>()
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Этичный Веб хакинг (web hacking) для начинающих",
                    Description = "Тестирование веб приложений на проникновение через различные уязвимости",
                    Students = new List<Guid>()
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "React + Next.js - с нуля. TypeScript, Hooks, SSR и CSS Grid",
                    Description = "Полный курс по современному Frontend на React и Next.js. Всё от CSS Grid и TypeScript до React Hooks и SSR",
                    Students = new List<Guid>()
                 //   Users = new() {"3445102c-8fc5-4be9-8e33-5bd229103f7d"}
                },
                new()
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                    Name = "Angular, Node, Express, Mongo. Создание Сервиса с Нуля",
                    Description = "Создай профессиональное Fullstack приложение, которое можно использовать как рабочий бизнес в реальной жизни!",
                    Students = new List<Guid>()
                    //   Price = 25000.00M,
                }
            };

            return courses;
        }
        public static IEnumerable<EduAnnouncement> GetCourseAnnounceSeedData(IEnumerable<EduCourse> courseData)
        {
            var Announcements = new List<EduAnnouncement>();

            for (int i = 0; i < courseData.Count(); i++)
            {
                switch (i)
                {
                    case 0:
                        Announcements.Add(new EduAnnouncement()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "Информация о курсе",
                            Description = "О чем данный курс?",
                            Content = "Здравствуйте студенты! Курс предназначен..",
                            Order = 1,
                            CourseId = courseData.ToArray()[i].Id,
                        });
                        break;
                    case 1:
                        Announcements.Add(new EduAnnouncement()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "Какой то ананос",
                            Content = "Чото там",
                            Order = 1,
                            CourseId = courseData.ToArray()[i].Id,
                        });
                        break;
                    case 2:
                        Announcements.Add(
                        new EduAnnouncement()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "Hello world",
                            Content = "Здравствуйте студенты!",
                            Order = 1,
                            CourseId = courseData.ToArray()[i].Id,
                        });
                        break;
                    case 3:
                        Announcements.Add(new EduAnnouncement()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "Место для лекций",
                            Content = "Здравствуйте студенты! Ссылка на проведения лекционных занятий - http://vk.com/12345",
                            Order = 1,
                            CourseId = courseData.ToArray()[i].Id,
                        });
                        break;
                }
            };
            return Announcements;
        }

        public static IEnumerable<EduModule> GetModuleSeedData(IEnumerable<EduCourse> courseData)
        {
            var Modules = new List<EduModule>();

            for (int i = 0; i < courseData.Count(); i++)
            {
                switch (i)
                {
                    case 0:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            Content = "Здравствуйте студенты! Данный модуль расскажет вам про модель OSI!",
                            CourseId = courseData.ToArray()[i].Id
                        }); ;
                        break;
                    case 1:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI1",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            Content = "Text, image",
                            CourseId = courseData.ToArray()[i].Id
                        });
                        break;
                    case 2:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI2",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            CourseId = courseData.ToArray()[i].Id
                        });
                        break;
                    case 3:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI3",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            CourseId = courseData.ToArray()[i].Id
                        });
                        break;
                    case 4:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI4",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            CourseId = courseData.ToArray()[i].Id
                        });
                        break;
                    case 5:
                        Modules.Add(new EduModule()
                        {
                            Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                            Name = "OSI5",
                            Description = "Данный модуль расскажет вам про модель OSI",
                            CourseId = courseData.ToArray()[i].Id
                        });
                        break;

                }
            }
            return Modules;
        }

        public static IEnumerable<EduAnnouncement> GetAnnounceSeedData(IEnumerable<EduModule> moduleData)
        {
            var Announcements = new List<EduAnnouncement>
                                {
                                    new EduAnnouncement()
                                    {
                                        Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                                        Name = "Доп инфо по модулю",
                                        Description = "",
                                        Content = "Здравствуйте студенты! Данный модуль расскажет вам подробно OSI!",
                                        Order = 1,
                                        CourseId = moduleData.ToArray()[0].CourseId,
                                        ModuleId = moduleData.ToArray()[0].Id
                                    }
            };
            return Announcements;
        }

        public static IEnumerable<EduTask> GetTaskSeedData(IEnumerable<EduModule> moduleData)
        {
            var Tasks = new List<EduTask>
                            {
                                new EduTask()
                                {
                                    Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                                    ModuleId = moduleData.ToArray()[0].Id,
                                    CourseId = moduleData.ToArray()[0].CourseId,
                                    Name = "Задание по OSI",
                                    Description = "Это задание позволит определить ваш базовый уровень знаний",
                                    Content = "Добавьте ответ на задание",
                                }
                            };
            return Tasks;
        }

        public static IEnumerable<EduTest> GetTestSeedData(IEnumerable<EduModule> moduleData)
        {
            var Tests = new List<EduTest>
                            {
                                new EduTest()
                                {
                                    ModuleId = moduleData.ToArray()[0].Id,
                                    CourseId = moduleData.ToArray()[0].CourseId,
                                    Id = MongoDB.Bson.ObjectId.GenerateNewId(DateTime.Now),
                                    Name = "Тест по OSI",
                                    Description = "Тест позволит определить ваш базовый уровень знаний",
                                    Content = "Пройдите тест",
                                    TestSettings = new EduTestSettings(){
                                        QuestionsAmount = 2,
                                        TimeToSolve = 300,  //300 = 5 минут
                                        ShowScoreOnFinish=false
                                    },
                                    Questions = new List<EduTestQuestion>
                                    {
                                        new EduTestQuestion()
                                        {
                                            Id = Guid.NewGuid(),
                                            Question = "Сетевая модель OSI это?",
                                            Variants = new List<EduTestAnswerVariant>
                                            {
                                                new ()
                                                {
                                                    Id = 1,
                                                    Answer = "Сетевая модель стека сетевых протоколов OSI/ISO. (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 2,
                                                    Answer = "Сетевая модель стека сетевых протоколов TCP/IP.",
                                                    IsCorrect = false
                                                },
                                                new ()
                                                {
                                                    Id = 3,
                                                    Answer = "Сетевая модель стека сетевых протоколов OSI.",
                                                    IsCorrect = false
                                                }
                                            },
                                            MultipleAnswers = false
                                        },
                                        new EduTestQuestion()
                                        {
                                            Id = Guid.NewGuid(),
                                            Question = "Количество уровней OSI?",
                                            Variants = new List<EduTestAnswerVariant>
                                            {
                                                new ()
                                                {
                                                    Id = 1,
                                                    Answer = "8",
                                                    IsCorrect = false
                                                },
                                                new ()
                                                {
                                                    Id = 2,
                                                    Answer = "7 (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 3,
                                                    Answer = "6",
                                                    IsCorrect = false
                                                }
                                            },
                                            MultipleAnswers = false
                                        },
                                        new EduTestQuestion()
                                        {
                                            Id = Guid.NewGuid(),
                                            Question = "Вопрос 1?",
                                            Variants = new List<EduTestAnswerVariant>
                                            {
                                                new ()
                                                {
                                                    Id = 1,
                                                    Answer = "Правильный ответ (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 2,
                                                    Answer = "Правильный ответ 2 (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 3,
                                                    Answer = "Неправильный ответ 3",
                                                    IsCorrect = false
                                                }
                                            },
                                            MultipleAnswers = true
                                        },
                                        new EduTestQuestion()
                                        {
                                            Id = Guid.NewGuid(),
                                            Question = "Вопрос 2?",
                                            Variants = new List<EduTestAnswerVariant>
                                            {
                                                new ()
                                                {
                                                    Id = 1,
                                                    Answer = "Правильный ответ (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 2,
                                                    Answer = "Правильный ответ 2 (TRUE)",
                                                    IsCorrect = true
                                                },
                                                new ()
                                                {
                                                    Id = 3,
                                                    Answer = "Неправильный ответ 3",
                                                    IsCorrect = false
                                                },
                                                new ()
                                                {
                                                    Id = 4,
                                                    Answer = "Неправильный ответ 3",
                                                    IsCorrect = false
                                                }
                                            },
                                            MultipleAnswers = true
                                        }
                                    }
                                }
                            };

            return Tests;
        }
    }
}