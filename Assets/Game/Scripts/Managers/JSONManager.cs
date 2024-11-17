using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils.Singleton;

public class JSONManager : DontDestroySingleton<JSONManager>
{
    string _todoListFilePath => Application.persistentDataPath + "/todolist.txt";
    string _studyTopicsFilePath => Application.persistentDataPath + "/studyTopics.txt";

    private TodoListManager _todoListManager => TodoListManager.I;
    
    #region To-do List Data
    public void SaveTodoList(List<TodoListObject> tasks, int amountListObjects)
    {
        string contents = "";
        
        for (int todoListObjectIndex = 0; todoListObjectIndex < amountListObjects; todoListObjectIndex++)
        {
            TodoListItem item = new TodoListItem(tasks[todoListObjectIndex].objName, tasks[todoListObjectIndex].topic, tasks[todoListObjectIndex].isChecked);
            contents += JsonUtility.ToJson(item) + "\n";
        }

        File.WriteAllText(_todoListFilePath, contents);
    }
    public void LoadTodoList()
    {
        if (File.Exists(_todoListFilePath))  
        {
            string contents = File.ReadAllText(_todoListFilePath);
        
            string[] splitContents = contents.Split('\n');
            
            foreach (string content in splitContents)
            {
                if(content.Trim() != "")
                {
                    TodoListItem temp = JsonUtility.FromJson<TodoListItem>(content);
                    _todoListManager.CreateTodoListItem(temp.objName, temp.topic, temp.isChecked, true);
                }
            }
        }
    }
    
    // Public class inside class to help with serialization
    public class TodoListItem
    {
        public string objName;
        public string topic;
        public bool isChecked;
    
        public TodoListItem(string name, string topic, bool isChecked)
        {
            this.objName = name;
            this.topic = topic;
            this.isChecked = isChecked;
        }
    }
    #endregion
    
    #region Study Topics Data
    
    public void SaveStudyTopics(string name, int amountListObjects)
    {
    }
    
    public void SaveStudyTopicTime(string name, int amountListObjects)
    {
    }
    
    public void ChangeStudyTopicName(string name, int amountListObjects)
    {
    }
    
    public void LoadStudyTopics()
    {
    }
    
    #endregion
}
