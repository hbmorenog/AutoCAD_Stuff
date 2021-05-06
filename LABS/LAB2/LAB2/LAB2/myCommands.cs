﻿// (C) Copyright 2021 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(LAB2.MyCommands))]

namespace LAB2
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("MyGroup", "MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        public void MyCommand() // This method can have any name
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;
            if (doc != null)
            {
                ed = doc.Editor;
                ed.WriteMessage("Hello, this is your first command.");

            }
        }

        // Modal Command with pickfirst selection
        [CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        // Application Session Command with localized name
        [CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string
        // or integer.
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }
        // 1. Add a command named addAnEnt. Use the CommandMethod attribute and a 
        // Public void function. 
        // Note: put the closing curley brace after step 21.

        [CommandMethod("addAnEnt")]
        public void AddAnEnt()
        {
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            // var db = doc.Database;

            // 2. Declare an Editor variable named ed. Instantiate it using the Editor property 
            // of the Application.DocumentManager.MdiActiveDocument.Editor 

            Editor ed = doc.Editor;

            // 3. Declare a PromptKeywordOptions variable and instantiate it by creating 
            // a new PromptKeywordOptions. Use a string similar to the following for the 
            // messageAndKeywords string. 
            // "Which entity do you want to create? [Circle/Block] : ", "Circle Block" 
            PromptKeywordOptions getWhichEntityOptions = new PromptKeywordOptions("Which entity do you want to create?[Circle / Block] : ", "Circle Block");
            
            // 4. Declare a PromptResult. Use the GetKeywords method of the Editor variable 
            // created in step 2. Pass in the PromptKeywordOptions created in step 3. Instantiate 
            // the PromptResult by making it equal to the return value of the GetKeywords method. 
            PromptResult getWhichEntityResult = ed.GetKeywords(getWhichEntityOptions);

            // 5. Add an if statement that tests the Status of the PromptResult created in step 4. 
            // Use the PromptStatus enum for the test. (see if it is equal to PromptStatus.OK) 
            // Note: Move the closing curly brace after step 21. 
            // (After the following instructions) 
            if (getWhichEntityResult.Status == PromptStatus.OK)
            {
                // 6. PromptStatus was ok. Now use a switch statement. For the switch argument
                // use the StringResult property of the PromptResult variable used above
                // Note: Move the closing curly brace after step 21.
                // (Above the closing curly brace for the if statement in step 5)
                switch (getWhichEntityResult.StringResult)
                {
                    // 7. Use "Circle" for the case. (if the StringResult is "Circle") Below 
                    // we will use "Block" for the case. (jump ahead to step 15 to add the break 
                    // to resolve the "Control cannot fall through... message") 

                    case "Circle":

                        // 8. We want to ask the user for the center of the circle. Declare 
                        // a PromptPointOptions variable and instatiate it by making it equal 
                        // to a new PromptPointOptions. Use "Pick Center Point : " for message parameter 
                        PromptPointOptions getPointOptions = new PromptPointOptions("Pick Center Point:");
                        
                        // 9. Declare a PromptPointResult variable. Use the GetPoint method of 
                        // the Editor created in step 2. (Pass in the PromptPointOptions created 
                        // in step 8). Instantiate the PromptPointResult by making it equal to the 
                        // return of the GetPoint method. 
                        PromptPointResult getPointResult= ed.GetPoint(getPointOptions);

                        // 10. Add an if statement that tests the Status of the PromptPointResult 
                        // created in step 9. Use the PromptStatus enum for the test. (make sure it is OK) 
                        // Note: Move the closing curly brace right before step 15. 
                        if (getPointResult.Status == PromptStatus.OK)
                        {
                            // 11. Now we want to ask the user for the radius of the circle. Declare 
                            // a PromptDistanceOptions variable. Instatiate it by making it equal 
                            // to a new PromptDistanceOptions. Use "Pick Radius : " for the message parameter. 
                            PromptDistanceOptions getRadiusOptions = new PromptDistanceOptions("Pick Radius:");

                            // 12. We want to use the point selected in step 9 as the 
                            // base point for the GetDistance call coming up. To do this use 
                            // the BasePoint property of the PromptDistanceOptions variable created 
                            // in the previous step. Make the BasePoint equal to the Value property 
                            // of the PromptPointResult created in step 9. 
                            getRadiusOptions.BasePoint = getPointResult.Value;

                            // 13. We need to tell the input mechanism to actually use the basepoint. 
                            // Do this by setting the UseBasePoint property of the 
                            // PromptDistanceOptions created in step 11 to True. 
                            getRadiusOptions.UseBasePoint = true;

                            // 14. Get the radius for the circle. Declare a PromptDoubleResult variable. 
                            // Instantiate it using the GetDistance method of the Editor variable created 
                            // in step 2. Pass in the PromptDistanceOptions created in step 11 and 
                            // modified in the previous steps. 
                            PromptDoubleResult getRadiusResult = ed.GetDistance(getRadiusOptions);
                        }

                        // 15. Add break to mark the end of the code for the "Circle" case. 
                        break;

                    case "Block":
                        // 17. Now we want to ask the user for the name of the block. Delcare 
                        // a PromptStringOptions varable and instatiate it by creating a new 
                        // PromptStringOptions. Use "Enter name of the Block to create : " for 
                        // the message parameter. 
                        PromptStringOptions blockNameOptions = new PromptStringOptions("Enter the name of the block to create:");

                        // 18. No spaces are allowed in a blockname so disable it. Do this by setting 
                        // the AllowSpaces property of the PromptStringOptions created in step 15 
                        // to false. 
                        blockNameOptions.AllowSpaces = false;

                        // 19. Get the name the user entered. Declare a PromptResult variable 
                        // and instantiate it using the GetString method of the Editor object 
                        // created in step 2. Pass in the PromptStringOptions created in step 17. 
                        PromptResult blockNameResult = ed.GetString(blockNameOptions);
                        
                        // 20. Add break to mark the end of the code for the "Block" case. 
                        break;
                }
            }

        }
    }

}
// 21. Build the project. Place a break point. Use the NETLOAD command 
// and run the AddAnEnt command. Step through the code and fix any errors. 
// Remember to run the command and test the code for both circle and block. 
// End of Lab2
