// (C) Copyright 2021 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(LAB3.MyCommands))]

namespace LAB3
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
                        PromptPointResult getPointResult = ed.GetPoint(getPointOptions);

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

                            if (getRadiusResult.Status == PromptStatus.OK)
                            {
                                // Begining of Lab3. Create the Circle or Block and BlockReference 
                                // 1. Declare a Database variable and instantiate it 
                                // using the Document.Database property of the editor created above. (ed) 
                                // Note: Add the Autodesk.AutoCAD.DatabaseServices; namespace for Database 
                                // and Transaction, use the using keyword (above the class declaration)
                                Database dwg = ed.Document.Database;

                                // 2. Declare a Transaction variable; instantiate it using the 
                                // TransactionManager.StartTransaction method of the Databse
                                // created in step 1.
                                Transaction trans = dwg.TransactionManager.StartTransaction();

                                // 3. Add a try, catch and finally block. Move the try closing curly
                                // brace right after step 8. Put the catch statement after this.
                                // Enclose  step 9 in the catch call. Enclose step 10 in the finally call.
                                // (Build the project and fix any problems).

                                try
                                {

                                    // 4. Declare a Circle variable and create it using the new keyword. 
                                    // Use the the Value property of the PromptPointResult created in 
                                    // Lab2 for the first parameter. For the second parameter (normal) use 
                                    // Vector3d.ZAxis. Use the Value property of the PromptDoubleResult 
                                    // (created in Lab2) for the radius. 
                                    // Note: Need to add Autodesk.AutoCAD.Geometry; namespace for Vector3d.
                                    Circle circle = new Circle(getPointResult.Value, Vector3d.ZAxis, getRadiusResult.Value);

                                    // 5. Declare a BlockTableRecord variable. Instatiate it using the 
                                    // GetObject method of the Transaction variable created in step 2. 
                                    // Use the CurrentSpaceId property of the Database variable created in 
                                    // step 1 for the first parameter. (ObjectId) For the second parameter 
                                    // use OpenMode.ForWrite. We are adding the circle to either ModelSpace 
                                    // or PaperSpace. (the CurrentSpaceId determines this) 
                                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);

                                    // 6. Add the Circle to the BlockTableRecord created in step 5. Use the 
                                    // AppendEntity method and pass in the circle created in step 4. 
                                    btr.AppendEntity(circle);

                                    // 7. Tell the transaction about the new circle so that it can autoclose 
                                    // it. Use the AddNewlyCreatedDBObject method. The first argument is the 
                                    // circle. Use True for the second argument. 
                                    trans.AddNewlyCreatedDBObject(circle, true);

                                    // 8. Commit the transaction by calling the Commit method. If the code gets 
                                    // this far everything should have worked correctly.
                                    trans.Commit();
                                }

                                catch (Autodesk.AutoCAD.Runtime.Exception e)
                                {

                                    // 9. Declare an Exception variable for the Catch. 
                                    // (add "(Exception ex)" to the catch keyword) 
                                    // Use the WriteMessage of the Editor variable (ed) created in Lab2. 
                                    // Use "problem due to " + ex.Message for the Message parameter. 
                                    // If an error occurs the details of the problem will be printed 
                                    // on the AutoCAD command line.
                                    ed.WriteMessage("Problem due to " + e.Message.ToString());
                                }

                                finally
                                {

                                    // 10. Dispose the transaction by calling the Dispose method 
                                    // of the Transaction created in step 2. This will be called 
                                    //whether an error on not occurred.
                                    trans.Dispose();
                                }
                            }
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

                        if (blockNameResult.Status == PromptStatus.OK)
                        {
                            // 11. Declare a Database variable and instantiate it using the 
                            // Document.Database property of the editor created above. (ed) 
                            Database dwg = ed.Document.Database;

                            // 12. Declare a Transaction variable; instantiate it using the 
                            // TransactionManager.StartTransaction method. 
                            Transaction trans = dwg.TransactionManager.StartTransaction();

                            // 13. Add a try, catch and finally block. Move the closing curly
                            // brace right after step 34. Put the catch statement after this.
                            // Enclose  step 35 in the catch call. Enclose step 36 in the finally call.
                            try
                            {
                            
                            // 14. Declare a BlockTableRecord variable. Create it using the 
                            // new keyword. 
                            BlockTableRecord newBlockDef = new BlockTableRecord();

                            // 15. Set the name of the BlockTableRecord. Use the 
                            // StringResult property of the PromptResult variable above. 
                            // (created in Lab2) 
                            newBlockDef.Name = blockNameResult.StringResult;

                            // 16. Declare a variable as a BlockTable. Instiate it using the 
                            // GetObject method of the Transaction. Use the BlockTableId property 
                            // of the Database variable created in step 11 for the first parameter. 
                            // Use OpenMode.ForRead for the second parameter. We are opening for 
                            // read to check if a block with the name provided by the user already exists. 
                            BlockTable blockTable = (BlockTable)trans.GetObject(dwg.BlockTableId, OpenMode.ForRead);

                            // 17. Add an if statement. Test to see if the BlockTable has a block by 
                            // using the Has method of the variable created in step 16. For the string 
                            // Parameter use the StringResult property of the PromptResult variable above. 
                            // created in Lab2. Check to see if it equals False. 
                            // Move the closing curly brace below Step 34. 
                            if (blockTable.Has(blockNameResult.StringResult) == false)
                                {
                            
                                // 18. The Block with that name does not exist so add it. 
                                // First make the BlockTable open for write. Do this by calling the 
                                // UpgradeOpen() method of the BlockTable. (created in step 16) 
                                blockTable.UpgradeOpen();

                                // 19. Add the BlockTableRecord created in step 14. Use the Add method 
                                // of the BlockTable and pass in the BlockTableRecord. 
                                blockTable.Add(newBlockDef);

                                // 20. Tell the transaction about the new Block so that it can autoclose 
                                // it. Use the AddNewlyCreatedDBObject method. The first argument is the 
                                // BlockTableRecord. Use true for the second argument. 
                                trans.AddNewlyCreatedDBObject(newBlockDef, true);

                                // 21. In the next two steps you add circles to the BlockTableRecord. 
                                // Declare a variable as a Circle and instantiate it using 
                                // the new Keyword. For the first argument create a new 
                                // Point3d use (0,0,0), for the second argument use Vector3d.ZAxis, 
                                // use 10 for the Radius argument. 
                                Circle circle1 = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, 10);

                                // 22. Append the circle to the BlockTableRecord. 
                                // Use the AppendEntity method pass in the circle from step 21 
                                newBlockDef.AppendEntity(circle1);

                                // 23. Now add another circle to the BlockTableRecord 
                                // Declare a variable as a Circle and instantiate it using 
                                // the new Keyword. For the first argument create a new 
                                // Point3d use (20,10,0), for the second arguement use Vector3d.ZAxis, 
                                // use 10 for the Radius argument.
                                Circle circle2 = new Circle(new Point3d(20, 10, 0), Vector3d.ZAxis, 10);

                                // 24. Append the second circle to the BlockTableRecord. 
                                // Use the AppendEntity method pass in the circle from step 23 
                                newBlockDef.AppendEntity(circle2);

                                // 25. Tell the transaction manager about the new objects so that 
                                // the transaction will autoclose them. Call the AddNewlyCreatedDBObject 
                                // pass in the Circle created in step 21. Do this again for the circle 
                                // Created in step 23. (use true for the second arguement). 
                                trans.AddNewlyCreatedDBObject(circle1, true);
                                trans.AddNewlyCreatedDBObject(circle2, true);

                                // 26. We have created a new block definition (BlockTableRecord). 
                                // Here we will use that Block and add a BlockReference to modelspace. 
                                // First declare a PromptPointOptions and instantiate it with the new 
                                // keyword. For the message parameter use "Pick insertion point of BlockRef : " 
                                PromptPointOptions blockRefPointOptions = new PromptPointOptions("Pick insertion point of BlockRef :");

                                // 27. Declare a PromptPointResult variable. Use the GetPoint method of 
                                // the Editor created in Lab2 (ed). Pass in the PromptPointOptions created 
                                // in step 26. 
                                PromptPointResult blockRefPointResult = ed.GetPoint(blockRefPointOptions);

                                // 28. Create an if statement and test the Status of the PromptPointResult. 
                                // Test if it is not equal to PromptStatus.OK. 
                                //Place the closing curly brace below step 30. 
                                if (blockRefPointResult.Status != PromptStatus.OK)
                                    {
                                    
                                    // 29. If we got here then the GetPoint failed. Call the dispose 
                                    // method of the Transaction created in step 11. 
                                    trans.Dispose();

                                    // 30. return 
                                    return;
                                    }

                                // 31. Declare a BlockReference variable. Instatiate it with the new keyword 
                                // Use the Value method of the PromptPointResult for the Position argument. (point3d) 
                                // Use the ObjectId property of the BlockTableRecord created in Step 14 for the 
                                // Second parameter. 
                                BlockReference blockRef = new BlockReference(blockRefPointResult.Value, newBlockDef.ObjectId);

                                // 32. Get the current space. (either ModelSpace or PaperSpace). 
                                // Declare a BlockTableRecord variable, instantiate it using the GetObject 
                                // method of the Transaction created in step 12. Use the CurrentSpaceId property 
                                // of the Database created in step 11. Open it for write. (OpenMode.ForWrite) 
                                BlockTableRecord curSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId,OpenMode.ForWrite);

                                // 33. Use the AppendEntity method of the BlockTableRecord created in step 32 
                                // and pass in the BlockReference created in step 31. 
                                curSpace.AppendEntity(blockRef);

                                // 34. Tell the transaction about the new block reference so that the transaction 
                                // can autoclose it. Use the AddNewlyCreatedDBObject of the Transaction created in 
                                // step 12. Pass in the BlockReference. Use true for the second parameter. 
                                trans.AddNewlyCreatedDBObject(blockRef, true);
                                }
                            
                            // 35. If the code makes it here then all is ok. Commit the transaction by calling 
                            // the Commit method
                            trans.Commit();

                            }
                            catch (Autodesk.AutoCAD.Runtime.Exception e)
                            {
                                // 36. Declare an Exception variable for the Catch. 
                                // (add "(Exception ex)" to the catch keyword) 
                                // Use the WriteMessage of the Editor variable (ed) created in Lab2. 
                                // Use "a problem occured because " + ex.Message for the Message parameter. 
                                // If an error occurs the details of the problem will be printed 
                                // on the AutoCAD command line.
                                ed.WriteMessage("Problem occured because " + e.Message.ToString());
                            }
                            finally
                            {
                                // 37. Dispose the transaction by calling the Dispose method 
                                // of the Transaction created in step 12. This will be called 
                                // whether an error on not occurred.  This is the end of Lab3.
                                // Build and debug by loading in AutoCAD. (use NETLOAD) and run
                                // the addAnEnt command.
                                trans.Dispose();
                            }
                        }

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
