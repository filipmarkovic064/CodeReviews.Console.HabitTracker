Habit Logger

Console based CRUD application to track Habits. Developed using C# and SQLite.
Given Requirements:


    When the application starts, it should create a sqlite database, if one isn’t present.
    It should also create a table in the database, where the habits will be logged.
    You are be able to insert, delete, update and view your habits.
    You should handle all possible errors so that the application never crashes
    The application should only be terminated when the user inserts 0.
    You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework


Features

    SQLite database connection
        The program uses a SQLite db connection to store and read information.
        If no database exists, or the correct table does not exist they will be created on program start.
        It seeds data into the db only the first time the program is run (using FirstRun.txt to check)


    A console based UI where users can navigate by key presses

    CRUD DB functions
        From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in dd-MM-yyyy format.
        Time and Dates inputted are checked to make sure they are in the correct and realistic format.
        
        
    What was hard? 
        Figuring out how to make the user be able to input any Habit without making additional tabels, still not sure if this is the best solution
        Figuring out how to make the program seed only the first time its run, had a few things i tested and in the end decided to do it by creating a .txt file that marks it.


    What was easy?
        Actually making it add, delete, update and stuff like that

     What have you learned?
        Learned a lot about SQLite, CRUD functions 

