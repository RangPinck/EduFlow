--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4 (Debian 17.4-1.pgdg120+2)
-- Dumped by pg_dump version 17.4 (Debian 17.4-1.pgdg120+2)
\connect edu_flow_db
SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: admin
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO admin;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: admin
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetRoles" (
    "Id" uuid NOT NULL,
    "IsNoManipulate" boolean NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


ALTER TABLE public."AspNetRoles" OWNER TO admin;

--
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL
);


ALTER TABLE public."AspNetUserRoles" OWNER TO admin;

--
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."AspNetUsers" (
    "Id" uuid NOT NULL,
    "UserDataCreate" timestamp with time zone NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL
);


ALTER TABLE public."AspNetUsers" OWNER TO admin;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO admin;

--
-- Name: blocks_materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.blocks_materials (
    bm_id uuid DEFAULT gen_random_uuid() NOT NULL,
    block uuid NOT NULL,
    material uuid NOT NULL,
    bm_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    note text,
    duration integer
);


ALTER TABLE public.blocks_materials OWNER TO admin;

--
-- Name: blocks_tasks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.blocks_tasks (
    task_id uuid DEFAULT gen_random_uuid() NOT NULL,
    task_name text NOT NULL,
    task_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    duration integer NOT NULL,
    block uuid NOT NULL,
    link text,
    task_number_of_block integer NOT NULL,
    description text
);


ALTER TABLE public.blocks_tasks OWNER TO admin;

--
-- Name: courses; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.courses (
    course_id uuid DEFAULT gen_random_uuid() NOT NULL,
    course_name text NOT NULL,
    course_data_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    description text,
    link text,
    author uuid NOT NULL
);


ALTER TABLE public.courses OWNER TO admin;

--
-- Name: courses_blocks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.courses_blocks (
    block_id uuid DEFAULT gen_random_uuid() NOT NULL,
    block_name text NOT NULL,
    block_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    course uuid NOT NULL,
    description text,
    block_number_of_course integer
);


ALTER TABLE public.courses_blocks OWNER TO admin;

--
-- Name: material_type; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.material_type (
    type_id integer NOT NULL,
    type_name text NOT NULL
);


ALTER TABLE public.material_type OWNER TO admin;

--
-- Name: material_type_type_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.material_type ALTER COLUMN type_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.material_type_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: materials; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.materials (
    material_id uuid DEFAULT gen_random_uuid() NOT NULL,
    material_name text NOT NULL,
    material_date_create timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    link text,
    type integer NOT NULL,
    description text
);


ALTER TABLE public.materials OWNER TO admin;

--
-- Name: study_states; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.study_states (
    state_id integer NOT NULL,
    state_name text NOT NULL
);


ALTER TABLE public.study_states OWNER TO admin;

--
-- Name: study_states_state_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.study_states ALTER COLUMN state_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.study_states_state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: tasks_practice; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.tasks_practice (
    practice_id uuid DEFAULT gen_random_uuid() NOT NULL,
    practice_name text NOT NULL,
    practice_date_created timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    duration integer NOT NULL,
    link text,
    task uuid NOT NULL,
    number_practice_of_task integer
);


ALTER TABLE public.tasks_practice OWNER TO admin;

--
-- Name: users; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users (
    user_id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_surname text NOT NULL,
    user_name text NOT NULL,
    user_patronymic text,
    is_first boolean DEFAULT true NOT NULL
);


ALTER TABLE public.users OWNER TO admin;

--
-- Name: users_courses; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users_courses (
    cu_id uuid DEFAULT gen_random_uuid() NOT NULL,
    "CourseId" uuid NOT NULL,
    "UserId" uuid NOT NULL
);


ALTER TABLE public.users_courses OWNER TO admin;

--
-- Name: users_tasks; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.users_tasks (
    ut_id uuid DEFAULT gen_random_uuid() NOT NULL,
    auth_user uuid NOT NULL,
    task uuid,
    practice uuid,
    material uuid,
    status integer NOT NULL,
    date_start timestamp with time zone NOT NULL,
    duration_task integer NOT NULL,
    duration_practice integer NOT NULL,
    duration_material integer NOT NULL
);


ALTER TABLE public.users_tasks OWNER TO admin;

--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetRoles" ("Id", "IsNoManipulate", "Name", "NormalizedName", "ConcurrencyStamp") FROM stdin;
f47ac10b-58cc-4372-a567-0e02b2c3d479	t	Ученик	УЧЕНИК	f47ac10b-58cc-4372-a567-0e02b2c3d479
c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e	t	Куратор	КУРАТОР	c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e
f45d2396-3e72-4ec7-b892-7bd454248158	t	Администратор	АДМИНИСТРАТОР	f45d2396-3e72-4ec7-b892-7bd454248158
\.


--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
33b58484-1bf5-4c42-ba72-2a53bbf67581	f45d2396-3e72-4ec7-b892-7bd454248158
0197372f-dee2-70ec-beac-4e0289ac004b	c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e
01973730-c379-7a2d-8bbd-25e30d5877fa	c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e
01973731-30e1-7c92-9d31-33233de1a9d3	f47ac10b-58cc-4372-a567-0e02b2c3d479
01973731-e212-7984-9847-55e1ab7bdcd4	f47ac10b-58cc-4372-a567-0e02b2c3d479
01973776-846a-741b-ab8f-252b982656fe	f47ac10b-58cc-4372-a567-0e02b2c3d479
019763ca-a9e1-7580-ab4a-2bfbe1a59af7	f47ac10b-58cc-4372-a567-0e02b2c3d479
\.


--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."AspNetUsers" ("Id", "UserDataCreate", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount") FROM stdin;
01973731-30e1-7c92-9d31-33233de1a9d3	2025-06-03 19:07:40.074516+00	alexey.smirnov_2025@yandex.ru	ALEXEY.SMIRNOV_2025@YANDEX.RU	alexey.smirnov_2025@yandex.ru	ALEXEY.SMIRNOV_2025@YANDEX.RU	t	AQAAAAIAAYagAAAAEK/aMFyMHKc7n1tQcBG73fHYSnved/KJaUyNS9WLIDontggNuohydo3jevB7QlLRkQ==	HGQEKBBHZRYXOSBL36WQQ7MWZ43W3LNY	93b12228-6772-46eb-b1f2-5dc4f6b51854	\N	f	f	\N	t	0
01973731-e212-7984-9847-55e1ab7bdcd4	2025-06-03 19:08:25.423012+00	elena.volkova99@mail.ru	ELENA.VOLKOVA99@MAIL.RU	elena.volkova99@mail.ru	ELENA.VOLKOVA99@MAIL.RU	t	AQAAAAIAAYagAAAAEAZNnWnigXjIeawUWjT7B9UA5idKwBenKJvvmNBNmelpFdHLbC/IfX+oSi4FAWmTVA==	FGT57HYDQ6WXFBPT65I3RQIKULNY2PIA	90ed46ef-e3a4-4e3a-af87-839271e51a38	\N	f	f	\N	t	0
01973776-846a-741b-ab8f-252b982656fe	2025-06-03 20:23:23.445226+00	svetlana.ivanova2025@mail.ru	SVETLANA.IVANOVA2025@MAIL.RU	svetlana.ivanova2025@mail.ru	SVETLANA.IVANOVA2025@MAIL.RU	t	AQAAAAIAAYagAAAAEK/yYBTp97KSvpwBmlQkFXrZVXuU0Sth0Wd0ryIfmwXzH5XOs5GLILINSzLux4cT/w==	WGKYMDFZWOXSSOACMBILVZ763HRWTPRD	1f27e63e-db38-467e-b880-51dd51437437	\N	f	f	\N	t	0
0197372f-dee2-70ec-beac-4e0289ac004b	2025-06-03 19:06:13.52672+00	ivan.petrov2025@example.com	IVAN.PETROV2025@EXAMPLE.COM	ivan.petrov2025@example.com	IVAN.PETROV2025@EXAMPLE.COM	f	AQAAAAIAAYagAAAAEKcwiYiBpQh80MiP3AmfHGmGI4afzNsEIVm3YLqn8C8z5AJQCAmkttXqoFqR5Rhj/w==	5OWBQXCDOANZ5RTQPOCKXLWURGYNXDYN	e8d08894-2dd2-44a9-89eb-347a787c398f	\N	f	f	\N	t	0
019763ca-a9e1-7580-ab4a-2bfbe1a59af7	2025-06-12 10:58:35.549689+00	user@user.com	USER@USER.COM	user@user.com	USER@USER.COM	t	AQAAAAIAAYagAAAAEOp6Un6ZuH83u0Wge+2PlYuAeOqqjJmVQg0tJ0haGd9wZFY00zassvhHguZD5ruv/A==	L5B5L64L6RSER4JRZQP4DAVB36JFM5D5	9544ddf0-27f3-438f-b473-3c17df9785b3	\N	f	f	\N	t	0
33b58484-1bf5-4c42-ba72-2a53bbf67581	2025-04-07 20:34:02.208429+00	adminadminadmin	ADMINADMINADMIN	admin@admin.com	ADMIN@ADMIN.COM	f	AQAAAAIAAYagAAAAEDAflT7y8miLvkMRoQzUwPtEZdPNY4F6ovKJDKCWpOnEfbr8CDDrjFXVU/gkUawR5w==	7AYC6LN67EUR2BL2SOWWMWAOGVFGDIVO	7d9253d0-2c07-4657-9039-4f7ef245c750	\N	f	f	\N	t	0
01973730-c379-7a2d-8bbd-25e30d5877fa	2025-06-03 19:07:12.062877+00	maria.kuznetsova87@gmail.com	MARIA.KUZNETSOVA87@GMAIL.COM	maria.kuznetsova87@gmail.com	MARIA.KUZNETSOVA87@GMAIL.COM	t	AQAAAAIAAYagAAAAEMTSFJ6uVRXxiFlewSV7GxjAN29MgDvmDXuFJX4/KIKPhSaxS4F28oA9yoBs0y6phQ==	GU7PRKFLNTPWKBUMHCUAVNH46W5ABKDQ	a38e81b2-d75c-4242-80ba-4acd4b4b6b82	\N	f	f	\N	t	0
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250407200419_Init	9.0.2
\.


--
-- Data for Name: blocks_materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_materials (bm_id, block, material, bm_date_create, note, duration) FROM stdin;
36bd2893-c325-439c-80c2-5f03015be858	6ad74d9a-bb02-49d8-8fe8-0ee8cc4696ae	660ed1f2-1ef3-4153-b97c-d985069439d7	2025-06-03 20:08:06.856086+00	\N	360
a5859923-ba5c-4f7e-ac8d-c3648761aad3	36c8556f-7cd1-417c-96f4-51132ff4f62d	d4b7ccf2-bf7b-41fa-90f2-a3f8244b6e63	2025-06-03 20:12:06.456677+00	\N	360
b5032f66-999e-4dbd-8d57-55180c543d0b	2c9d051a-2f99-4d68-baa3-1ad6bd270e02	5397bb2a-7102-4b2d-8c50-cd388b9a06ef	2025-06-03 20:16:14.33212+00	\N	360
941893c8-47cd-4365-9e0a-46a9f24f9687	1846fd7f-0d9a-4424-bb1d-dce16698d778	5d9fc1eb-687d-4da0-947e-f0f2948abf7a	2025-06-03 20:21:56.890215+00	\N	360
4063978a-46b3-4a79-a79b-d43128320a14	4a53e29d-89b5-4095-adaa-66d4d3aa35cb	1bdd891f-9a87-4dcf-8bf2-948f52b4c540	2025-06-12 11:08:28.673405+00	\N	960
\.


--
-- Data for Name: blocks_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.blocks_tasks (task_id, task_name, task_date_created, duration, block, link, task_number_of_block, description) FROM stdin;
73090e06-f2b1-4ab4-b907-1d8d8931c338	Двоичное представление информации	2025-06-03 19:29:57.931947+00	90	6ad74d9a-bb02-49d8-8fe8-0ee8cc4696ae		1	Понимание двоичной системы счисления, перевод чисел между системами, двоичная арифметика.
f924b679-8768-4de0-8210-3f32a07a6caa	Кодирование информации	2025-06-03 19:31:47.016093+00	90	6ad74d9a-bb02-49d8-8fe8-0ee8cc4696ae	\N	2	Изучение способов кодирования текстовой, графической и звуковой информации, кодовых таблиц.
a2a7c232-388c-4402-a36d-c4534224087b	Основы алгебры логики	2025-06-03 19:32:41.065347+00	90	6ad74d9a-bb02-49d8-8fe8-0ee8cc4696ae	\N	3	Изучение логических операций, законов алгебры логики и их применения в информатике.
ed981993-620d-4b32-8af1-e9278272f1bf	Структуры алгоритмов и базовое программирование	2025-06-03 20:08:43.62889+00	90	36c8556f-7cd1-417c-96f4-51132ff4f62d		1	Изучение линейных алгоритмов, операторов присваивания, ввода и вывода данных.
3f85966a-5174-4c92-b0f2-6bd40d522960	Программирование ветвлений и циклов	2025-06-03 20:10:23.379911+00	90	36c8556f-7cd1-417c-96f4-51132ff4f62d	\N	2	Освоение условных операторов и циклических конструкций для решения задач.
85fb4c2e-7338-48b0-93ee-ec1d11b22efe	Массивы и обработка данных	2025-06-03 20:11:13.823888+00	90	36c8556f-7cd1-417c-96f4-51132ff4f62d	\N	3	Работа с массивами, поиск максимума и минимума, сортировка данных.
44265e01-7e8c-463c-892f-865f559def6f	Электронные таблицы и вычисления	2025-06-03 20:13:32.612946+00	90	2c9d051a-2f99-4d68-baa3-1ad6bd270e02	\N	1	Создание и использование таблиц, формул и диаграмм для анализа данных.
bd853a6e-150e-402d-beeb-5f542ad49a91	Основы баз данных и запросы	2025-06-03 20:14:34.330483+00	90	2c9d051a-2f99-4d68-baa3-1ad6bd270e02	\N	2	Ознакомление с понятием базы данных, реляционной моделью и основами запросов.
730ef09d-8a16-4946-bdcb-605430fc270b	Интернет и веб-технологии	2025-06-03 20:15:19.557064+00	90	2c9d051a-2f99-4d68-baa3-1ad6bd270e02	\N	3	Изучение основ работы в интернете, создание простых веб-страниц и использование интернет-сервисов.
b9437033-0d09-4b9a-819e-43bcd16812bc	Основы компьютерных сетей	2025-06-03 20:19:22.459655+00	90	1846fd7f-0d9a-4424-bb1d-dce16698d778	\N	1	Понятие сетей, типы, протоколы и адресация.
98c270d8-615a-40e8-a828-a9a703510a27	Информационная безопасность	2025-06-03 20:20:17.52515+00	90	1846fd7f-0d9a-4424-bb1d-dce16698d778	\N	2	Методы защиты данных, создание паролей, антивирусные средства.
607ecd1a-13f8-4f3c-8ec6-5e25c35ed3e1	Цифровая этика и безопасное поведение	2025-06-03 20:21:06.091602+00	90	1846fd7f-0d9a-4424-bb1d-dce16698d778	\N	3	Правила поведения в интернете, предотвращение кибербуллинга.
7201d859-4e36-4dee-a0d2-639e4e912eea	Начало биологии	2025-06-12 10:57:05.618466+00	45	7ea823b3-478b-4258-b973-6f811d1943a9	\N	1	Изучение основ биологического развития организмов
9524490c-b6d9-4ce6-828c-9cbb205b6c34	Как появилась жизнь на земле	2025-06-12 11:07:40.27997+00	45	4a53e29d-89b5-4095-adaa-66d4d3aa35cb	\N	1	Узнать, как появилиси и развивали организмы
\.


--
-- Data for Name: courses; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses (course_id, course_name, course_data_create, description, link, author) FROM stdin;
3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	Информатика 11 класс	2025-06-03 19:12:08.27344+00	Этот курс предназначен для школьников 11 класса и охватывает ключевые темы базового уровня информатики, направленные на формирование функциональной цифровой грамотности и практических навыков работы с информацией и технологиями.	https://forms.gle/bmyM6eUAz3J7aNRb9	0197372f-dee2-70ec-beac-4e0289ac004b
e37edb0c-e8b2-48c0-bc6d-cd1e345b9c51	Биология. 7 класс	2025-06-12 10:55:48.453282+00	Курс по биологии	\N	33b58484-1bf5-4c42-ba72-2a53bbf67581
794489f8-a98b-4e78-aa59-fa211203db66	Биология. 5 класс	2025-06-12 11:06:38.111282+00	Биология, Вводный курс	https://www.youtube.com/watch?v=R3pm6xVh4_Y	33b58484-1bf5-4c42-ba72-2a53bbf67581
\.


--
-- Data for Name: courses_blocks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.courses_blocks (block_id, block_name, block_date_created, course, description, block_number_of_course) FROM stdin;
6ad74d9a-bb02-49d8-8fe8-0ee8cc4696ae	Теоретические основы информатики	2025-06-03 19:27:36.456928+00	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	Краткое описание: Изучение основных понятий информатики, способов представления и кодирования информации, двоичных систем счисления и основ алгебры логики. Формирование базовых знаний о цифровом представлении данных и алгоритмах.	1
36c8556f-7cd1-417c-96f4-51132ff4f62d	Алгоритмы и программирование	2025-06-03 20:08:22.569211+00	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	Формирование алгоритмического мышления, изучение структур алгоритмов, операторов ветвления и циклов, базовых навыков программирования на языке высокого уровня.	2
2c9d051a-2f99-4d68-baa3-1ad6bd270e02	 Информационные технологии и базы данных	2025-06-03 20:13:16.619343+00	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	Изучение прикладных информационных технологий, работа с электронными таблицами, базами данных и интернет-сервисами.	3
1846fd7f-0d9a-4424-bb1d-dce16698d778	Компьютерные сети и информационная безопасность	2025-06-03 20:19:05.342347+00	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	Основы компьютерных сетей, организация Интернета, вопросы информационной безопасности и этики в цифровом пространстве.	4
7ea823b3-478b-4258-b973-6f811d1943a9	Теория дарвина	2025-06-12 10:56:24.972227+00	e37edb0c-e8b2-48c0-bc6d-cd1e345b9c51	\N	1
4a53e29d-89b5-4095-adaa-66d4d3aa35cb	Теория Дарвина	2025-06-12 11:06:59.783351+00	794489f8-a98b-4e78-aa59-fa211203db66	\N	1
\.


--
-- Data for Name: material_type; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.material_type (type_id, type_name) FROM stdin;
1	Теория
2	Практика
3	Профессиональная подготовка
\.


--
-- Data for Name: materials; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.materials (material_id, material_name, material_date_create, link, type, description) FROM stdin;
9bf8ed76-1bd7-4243-8ead-3be51e1b59a7	Книга 101 совет начинающим разработчикам в системе "1С:Предприятие 8"	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/xJR4Uq74xbowNQ	1	
b96e5f8b-b37d-4d0c-8043-6479869593ec	Язык запросов "1С:Предприятие 8"  E.Ю Хрусталёва	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/P19Rll4HU0MpcQ	1	
68e6b362-e2ae-4c11-a021-dc2ddd99063f	Книга Радченко "1С:Программирование для начинающих Разработка в системе 1С:П 8.3"	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/Y5t__LB6BFXs8Q	1	
9f26e200-22d4-4da5-862c-35ae4b78acae	Система компоновки данных "1С:Предприятие 8" E.Ю Хрусталёва	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/PX4FR1DuVI98_A	1	
e3ef10b9-6d4d-4506-827a-f4f984753349	Начинайте учить профку тут 	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
fc514bba-adec-4030-aeb6-5b0312d53249	Не забывай про профку :)	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
c0360a37-3d6e-4d0b-8cd1-838540d6293b	Учи профку - последний месяц :0	2025-04-08 09:48:07+00	http://edu.koderline-soft.ru/demand/view/?token=f1e33c87bb62f21ae5c3cb1345f0600f&eauth=012eb621e28ab71aa091f4459156ae65	3	
c57fde5f-9b35-4fec-aa2f-f7baaa23fbc1	Выполнение учебных задач по разработанной конфигурации по книге "Радченко".	2025-04-08 09:48:07+00		2	
a2c833a9-9dc8-4e91-b93d-35d56010a5dc	Программирование за 21 день (курсы-по-1с.рф) 20 часов вместе с дз	2025-04-08 09:48:07+00	https://курсы-по-1с.рф/free/programming-in-1c-in-21-days/final-all-in-one/	2	
eee6ec87-1dbd-47b5-8fc6-cf8c4d1554f7	Онлайн-школа 1С программирования Евгения Милькина (без седьмого блока) 	2025-04-08 09:48:07+00	https://helpme1s.ru/shkola-programmirovaniya-v-1s	2	
7f34a6d3-b773-4b7a-adb7-4f59e3768d98	Книга Радченко "Практическое пособие разработчика" (кроме 16 - 18 глав).	2025-04-08 09:48:07+00	https://disk.yandex.ru/i/zOMO1RilllscAA	2	
0c84ec7d-c6c9-4c7a-b6a7-0740741f13d6	Материал	2025-05-29 02:54:25.355802+00	https://www.youtube.com/watch?v=t5FBwq-kudw	1	\N
660ed1f2-1ef3-4153-b97c-d985069439d7	Теоретические основы информатики (лекции и практические материалы)	2025-06-03 20:08:06.821636+00	https://infourok.ru/struktura-soderzhaniya-bazovogo-kursa-informatiki-4758412.html	1	\N
d4b7ccf2-bf7b-41fa-90f2-a3f8244b6e63	Алгоритмы и программирование (учебные материалы и практикумы)	2025-06-03 20:12:06.449829+00	https://edsoo.ru/wp-content/uploads/2023/08/21_%D0%A4%D0%A0%D0%9F-%D0%98%D0%BD%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B8%D0%BA%D0%B0_10-11-%D0%BA%D0%BB%D0%B0%D1%81%D1%81%D1%8B_%D0%B1%D0%B0%D0%B7%D0%B0.pdf	1	\N
5397bb2a-7102-4b2d-8c50-cd388b9a06ef	 Информационные технологии и базы данных	2025-06-03 20:16:14.338049+00	https://infourok.ru/uchebnaya-programma-po-informatike-klass-bazoviy-uroven-2860604.html	1	\N
5d9fc1eb-687d-4da0-947e-f0f2948abf7a	Компьютерные сети и безопасность	2025-06-03 20:21:56.876379+00	https://stepik.org/course/54449	1	\N
1bdd891f-9a87-4dcf-8bf2-948f52b4c540	Учебник по биологии. 5 класс	2025-06-12 11:08:23.632419+00	https://www.youtube.com/watch?v=R3pm6xVh4_Y	1	Учебни по биологии для средних классов
\.


--
-- Data for Name: study_states; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.study_states (state_id, state_name) FROM stdin;
1	Не приступал к изучению
2	Начал изучать
3	Изучил
\.


--
-- Data for Name: tasks_practice; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.tasks_practice (practice_id, practice_name, practice_date_created, duration, link, task, number_practice_of_task) FROM stdin;
18ab7280-73c6-495b-8d32-157255f71a7c	Рассмотреть примеры и выявить признаки фишинга.	2025-06-03 20:20:45.988984+00	30	\N	98c270d8-615a-40e8-a828-a9a703510a27	22
17fd2072-6f1f-4775-8c2b-f4dbf9b9e655	Составить список правил безопасного и этичного поведения в сети.	2025-06-03 20:21:24.567686+00	30	\N	607ecd1a-13f8-4f3c-8ec6-5e25c35ed3e1	23
60ddb029-e747-45de-a4d1-3e905481a0a0	Выполнить перевод 10 чисел из десятичной системы в двоичную и обратно.	2025-06-03 19:30:54.403396+00	30	\N	73090e06-f2b1-4ab4-b907-1d8d8931c338	1
95d34ec3-6007-4c7f-94ba-aa6b5485752a	Выполнить 5 примеров сложения и вычитания в двоичной системе.	2025-06-03 19:31:10.548374+00	30	\N	73090e06-f2b1-4ab4-b907-1d8d8931c338	2
5493b70f-56ae-49b6-8e00-7ccd50a42ba8	Закодировать короткий текст с помощью таблицы ASCII	2025-06-03 19:32:09.963812+00	30	\N	f924b679-8768-4de0-8210-3f32a07a6caa	3
900c798f-f5e8-4ab2-86fa-be7e723fba2a	Рассмотреть особенности кодирования BMP и WAV файлов.	2025-06-03 19:32:21.461908+00	30	\N	f924b679-8768-4de0-8210-3f32a07a6caa	4
7491cfc6-f67d-4095-bb47-6ae694e405c0	Составить таблицы истинности для 5 логических выражений.	2025-06-03 19:32:56.927905+00	30	\N	a2a7c232-388c-4402-a36d-c4534224087b	5
780d07cd-7cdf-468b-b5b8-0fea98de2233	Преобразование логических выражений с использованием законов алгебры логики. Упростить 3 логических выражения.	2025-06-03 19:33:13.842087+00	30	\N	a2a7c232-388c-4402-a36d-c4534224087b	6
5f974c5f-46fb-4ba7-9fff-b0295447d4ad	Создать программу, которая запрашивает имя пользователя и выводит приветствие.	2025-06-03 20:09:00.97941+00	30	\N	ed981993-620d-4b32-8af1-e9278272f1bf	7
2e569376-adce-4b69-9522-fd50e33ec455	Написать программу, принимающую длину и ширину и выводящую площадь.	2025-06-03 20:09:11.551117+00	30	\N	ed981993-620d-4b32-8af1-e9278272f1bf	8
b025a212-d82e-4906-a1ae-1556bfbbbdda	Написать программу, которая определяет, является ли число положительным, отрицательным или нулём.	2025-06-03 20:10:36.094834+00	30	\N	3f85966a-5174-4c92-b0f2-6bd40d522960	9
2bf47fde-305f-49db-afa2-f30b310e3620	Использовать цикл для последовательного вывода чисел.	2025-06-03 20:10:50.908812+00	30	\N	3f85966a-5174-4c92-b0f2-6bd40d522960	10
586a92ca-afb8-4804-8e11-e9f1bbacc10c	Создать массив из 10 чисел и вывести их на экран.	2025-06-03 20:11:28.689559+00	30	\N	85fb4c2e-7338-48b0-93ee-ec1d11b22efe	11
1edc42aa-01df-48c4-af3a-fd84891a6c8f	Написать программу для нахождения максимума.	2025-06-03 20:11:37.55987+00	30	\N	85fb4c2e-7338-48b0-93ee-ec1d11b22efe	12
a8da4cd1-4c4b-440c-9c21-7b396590357d	Ввести данные о расходах и подсчитать итоговые суммы.	2025-06-03 20:13:48.157897+00	30	\N	44265e01-7e8c-463c-892f-865f559def6f	13
f34e295f-0182-4e1f-9d84-0fde679c3aba	Ознакомление с понятием базы данных, реляционной моделью и основами запросов.	2025-06-03 20:14:00.026066+00	90	\N	44265e01-7e8c-463c-892f-865f559def6f	14
a1f268b5-6ca2-4f15-9ac7-bc07793ffc3d	Создать таблицу с данными о студентах.	2025-06-03 20:14:51.411695+00	30	\N	bd853a6e-150e-402d-beeb-5f542ad49a91	15
854af878-64f1-41ed-acf4-0e60b649e44a	Написать запрос для выборки студентов старше 16 лет.	2025-06-03 20:15:00.412503+00	30	\N	bd853a6e-150e-402d-beeb-5f542ad49a91	16
415bb3a2-5ffc-44da-b40a-5eda87739191	Создание простой веб-страницы с HTML. Написать страницу с заголовком, текстом и изображением.	2025-06-03 20:15:42.960836+00	30	\N	730ef09d-8a16-4946-bdcb-605430fc270b	17
a686e3b1-3e8b-460a-ac88-5cf42e621abc	Создать почтовый ящик, отправить и получить письмо с вложением.	2025-06-03 20:15:56.352981+00	30	\N	730ef09d-8a16-4946-bdcb-605430fc270b	18
a1f2ee47-0190-452b-8aa0-67da4ca3496f	Изучение структуры локальной сети и её компонентов. Построить схему локальной сети. 	2025-06-03 20:19:41.866965+00	30	\N	b9437033-0d09-4b9a-819e-43bcd16812bc	19
1bcd685e-479b-4fc3-85bc-99e0cb1aef75	Найти IP-адрес компьютера и выполнить команду ping.	2025-06-03 20:19:57.915491+00	30	\N	b9437033-0d09-4b9a-819e-43bcd16812bc	20
066806f3-eb0e-4aaf-9888-c53ae50d77ec	Разработать и сохранить пароль, активировать 2FA.	2025-06-03 20:20:34.459163+00	30	\N	98c270d8-615a-40e8-a828-a9a703510a27	21
ba972109-c8e2-4b9e-a5ee-990b26efa30e	Анализ ситуаций кибербуллинга и способов реагирования. Рассмотреть кейсы и предложить меры.	2025-06-03 20:21:38.428504+00	30	\N	607ecd1a-13f8-4f3c-8ec6-5e25c35ed3e1	24
8b14a98b-3532-47c2-91b0-f75615def487	Посмотртреть видео	2025-06-12 10:57:48.455983+00	30	https://www.youtube.com/watch?v=R3pm6xVh4_Y	7201d859-4e36-4dee-a0d2-639e4e912eea	25
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users (user_id, user_surname, user_name, user_patronymic, is_first) FROM stdin;
33b58484-1bf5-4c42-ba72-2a53bbf67581	admin	admin		f
01973730-c379-7a2d-8bbd-25e30d5877fa	Кузнецова	Мария 	Ефремовна	t
01973731-30e1-7c92-9d31-33233de1a9d3	Смирнов	Алексей 	Игоревич	t
01973731-e212-7984-9847-55e1ab7bdcd4	Волкова	Елена	Викторовна	t
01973776-846a-741b-ab8f-252b982656fe	Иванова	Светлана 	\N	t
0197372f-dee2-70ec-beac-4e0289ac004b	Сидоров	Иван	Игоревич	t
019763ca-a9e1-7580-ab4a-2bfbe1a59af7	Пользователь	Пользователь	\N	t
\.


--
-- Data for Name: users_courses; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users_courses (cu_id, "CourseId", "UserId") FROM stdin;
be12ffc8-70f5-4ffd-88ca-7dc747ec6011	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	01973731-30e1-7c92-9d31-33233de1a9d3
20345f6b-dc89-48d7-b717-008817aecd95	3657d3ba-cb6f-42a9-a71c-431b27c5fa9d	01973731-e212-7984-9847-55e1ab7bdcd4
df6aed47-771e-4a66-91e9-e5e8ab67b475	e37edb0c-e8b2-48c0-bc6d-cd1e345b9c51	01973731-30e1-7c92-9d31-33233de1a9d3
4df7c7db-6ca1-45f6-8a37-949d4d3beeb5	e37edb0c-e8b2-48c0-bc6d-cd1e345b9c51	01973731-e212-7984-9847-55e1ab7bdcd4
\.


--
-- Data for Name: users_tasks; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.users_tasks (ut_id, auth_user, task, practice, material, status, date_start, duration_task, duration_practice, duration_material) FROM stdin;
1658ae4c-8ce2-4c9c-8dc0-759e79ac6902	0197372f-dee2-70ec-beac-4e0289ac004b	\N	60ddb029-e747-45de-a4d1-3e905481a0a0	\N	3	2025-06-03 20:18:04.043591+00	0	40	0
d9f3cef6-e2a2-415c-9a82-3ef2cd175a40	0197372f-dee2-70ec-beac-4e0289ac004b	\N	\N	941893c8-47cd-4365-9e0a-46a9f24f9687	3	2025-06-03 20:22:23.740092+00	0	0	90
4c6ea0d3-d2ec-413a-a731-99d7815b5848	33b58484-1bf5-4c42-ba72-2a53bbf67581	\N	60ddb029-e747-45de-a4d1-3e905481a0a0	\N	3	2025-06-12 11:00:17.067099+00	0	45	0
\.


--
-- Name: material_type_type_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.material_type_type_id_seq', 3, true);


--
-- Name: study_states_state_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.study_states_state_id_seq', 3, true);


--
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: courses_blocks pk_block; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks
    ADD CONSTRAINT pk_block PRIMARY KEY (block_id);


--
-- Name: blocks_materials pk_blocks_materials; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT pk_blocks_materials PRIMARY KEY (bm_id);


--
-- Name: courses pk_course; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses
    ADD CONSTRAINT pk_course PRIMARY KEY (course_id);


--
-- Name: materials pk_material; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT pk_material PRIMARY KEY (material_id);


--
-- Name: material_type pk_material_type; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.material_type
    ADD CONSTRAINT pk_material_type PRIMARY KEY (type_id);


--
-- Name: tasks_practice pk_practice; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.tasks_practice
    ADD CONSTRAINT pk_practice PRIMARY KEY (practice_id);


--
-- Name: study_states pk_state_type; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.study_states
    ADD CONSTRAINT pk_state_type PRIMARY KEY (state_id);


--
-- Name: blocks_tasks pk_task; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_tasks
    ADD CONSTRAINT pk_task PRIMARY KEY (task_id);


--
-- Name: users pk_user; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_user PRIMARY KEY (user_id);


--
-- Name: users_courses pk_users_courses; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT pk_users_courses PRIMARY KEY (cu_id);


--
-- Name: users_tasks pk_ut; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT pk_ut PRIMARY KEY (ut_id);


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- Name: IX_blocks_materials_block; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_materials_block" ON public.blocks_materials USING btree (block);


--
-- Name: IX_blocks_materials_material; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_materials_material" ON public.blocks_materials USING btree (material);


--
-- Name: IX_blocks_tasks_block; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_blocks_tasks_block" ON public.blocks_tasks USING btree (block);


--
-- Name: IX_courses_author; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_courses_author" ON public.courses USING btree (author);


--
-- Name: IX_courses_blocks_course; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_courses_blocks_course" ON public.courses_blocks USING btree (course);


--
-- Name: IX_materials_type; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_materials_type" ON public.materials USING btree (type);


--
-- Name: IX_tasks_practice_task; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_tasks_practice_task" ON public.tasks_practice USING btree (task);


--
-- Name: IX_users_courses_CourseId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_courses_CourseId" ON public.users_courses USING btree ("CourseId");


--
-- Name: IX_users_courses_UserId; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_courses_UserId" ON public.users_courses USING btree ("UserId");


--
-- Name: IX_users_tasks_auth_user; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_auth_user" ON public.users_tasks USING btree (auth_user);


--
-- Name: IX_users_tasks_material; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_material" ON public.users_tasks USING btree (material);


--
-- Name: IX_users_tasks_practice; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_practice" ON public.users_tasks USING btree (practice);


--
-- Name: IX_users_tasks_status; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_status" ON public.users_tasks USING btree (status);


--
-- Name: IX_users_tasks_task; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX "IX_users_tasks_task" ON public.users_tasks USING btree (task);


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: users FK_users_AspNetUsers_user_id; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "FK_users_AspNetUsers_user_id" FOREIGN KEY (user_id) REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: blocks_materials fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id) ON DELETE CASCADE;


--
-- Name: blocks_tasks fk_bm_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_tasks
    ADD CONSTRAINT fk_bm_blocks FOREIGN KEY (block) REFERENCES public.courses_blocks(block_id) ON DELETE CASCADE;


--
-- Name: blocks_materials fk_bm_materials; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.blocks_materials
    ADD CONSTRAINT fk_bm_materials FOREIGN KEY (material) REFERENCES public.materials(material_id) ON DELETE CASCADE;


--
-- Name: courses_blocks fk_courses_blocks; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses_blocks
    ADD CONSTRAINT fk_courses_blocks FOREIGN KEY (course) REFERENCES public.courses(course_id) ON DELETE CASCADE;


--
-- Name: users_courses fk_cu_courses; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT fk_cu_courses FOREIGN KEY ("CourseId") REFERENCES public.courses(course_id) ON DELETE CASCADE;


--
-- Name: users_courses fk_cu_users; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_courses
    ADD CONSTRAINT fk_cu_users FOREIGN KEY ("UserId") REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: materials fk_materials_types; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT fk_materials_types FOREIGN KEY (type) REFERENCES public.material_type(type_id) ON DELETE CASCADE;


--
-- Name: tasks_practice fk_practice_task; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.tasks_practice
    ADD CONSTRAINT fk_practice_task FOREIGN KEY (task) REFERENCES public.blocks_tasks(task_id) ON DELETE CASCADE;


--
-- Name: courses fk_user_course; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.courses
    ADD CONSTRAINT fk_user_course FOREIGN KEY (author) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_material; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_material FOREIGN KEY (material) REFERENCES public.blocks_materials(bm_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_practice; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_practice FOREIGN KEY (practice) REFERENCES public.tasks_practice(practice_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_status; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_status FOREIGN KEY (status) REFERENCES public.study_states(state_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_task; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_task FOREIGN KEY (task) REFERENCES public.blocks_tasks(task_id) ON DELETE CASCADE;


--
-- Name: users_tasks fk_users_tasks_user; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.users_tasks
    ADD CONSTRAINT fk_users_tasks_user FOREIGN KEY (auth_user) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: admin
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

