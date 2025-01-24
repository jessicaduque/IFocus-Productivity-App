using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils.Singleton;

public class JSONManager : DontDestroySingleton<JSONManager>
{
    string _todoListFilePath => Application.persistentDataPath + "/todolist.txt";
    string _studyTopicsFilePath => Application.persistentDataPath + "/studyTopics.txt";

    private TodoListManager _todoListManager => TodoListManager.I;
    private StudyTopicsManager _StudyTopicsManager => StudyTopicsManager.I;

    protected override void Awake()
    {
        base.Awake();

        InitialGameCheck();
    }

    private void InitialGameCheck()
    {
        // Inicialização tópicos de estudo
        if (!File.Exists(_studyTopicsFilePath) || File.ReadAllText(_studyTopicsFilePath) == string.Empty)
        {
            StudyTopicItem item = new StudyTopicItem("Default", true, 0);
            File.WriteAllText(_studyTopicsFilePath, JsonUtility.ToJson(item) + "\n");
        }
    }

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
    
    public void SaveStudyTopics(List<StudyTopic> topics, int amountListObjects)
    {
        string contents = "";
        
        for (int amountListObjectsIndex = 0; amountListObjectsIndex < amountListObjects; amountListObjectsIndex++)
        {
            StudyTopicItem item = new StudyTopicItem(topics[amountListObjectsIndex].GetObjName(), topics[amountListObjectsIndex].GetIsDefault(), topics[amountListObjectsIndex].GetTimeStudiedTotalSeconds());
            contents += JsonUtility.ToJson(item) + "\n";
        }

        File.WriteAllText(_studyTopicsFilePath, contents);
    }
    
    // public void SaveStudyTopicTime(string name, int timeSeconds)
    // {
    // }
    
    public void ChangeStudyTopicName(string originalName, string newName)
    {
        string contentsToLoad = File.ReadAllText(_studyTopicsFilePath);
        string contentsToSave = "";
        
        string[] splitContents = contentsToLoad.Split('\n');
        
        foreach (string content in splitContents)
        {
            if(content.Trim() != "")
            {
                StudyTopicItem temp = JsonUtility.FromJson<StudyTopicItem>(content);
                if (temp.objName == originalName)
                {
                    temp.objName = newName;
                }
                contentsToSave += JsonUtility.ToJson(temp) + "\n";
            }
        }
        
        File.WriteAllText(_studyTopicsFilePath, contentsToSave);
        
    }
    
    public void DeleteStudytopic(string topicName)
    {
        string contentsToLoad = File.ReadAllText(_studyTopicsFilePath);
        string contentsToSave = "";
        
        string[] splitContents = contentsToLoad.Split('\n');
        
        foreach (string content in splitContents)
        {
            if(content.Trim() != "")
            {
                StudyTopicItem temp = JsonUtility.FromJson<StudyTopicItem>(content);
                if (temp.objName != topicName)
                {
                    contentsToSave += JsonUtility.ToJson(temp) + "\n";
                }
            }
        }
        
        File.WriteAllText(_studyTopicsFilePath, contentsToSave);
    }
    
    public void LoadStudyTopics()
    {
        if (File.Exists(_studyTopicsFilePath))  
        {
            string contents = File.ReadAllText(_studyTopicsFilePath);
        
            string[] splitContents = contents.Split('\n');
            
            foreach (string content in splitContents)
            {
                if(content.Trim() != "")
                {
                    StudyTopicItem temp = JsonUtility.FromJson<StudyTopicItem>(content);
                    _StudyTopicsManager.CreateStudyTopicItem(temp.objName, true, temp.isDefault);
                }
            }
        }
    }
    
    // Public class inside class to help with serialization
    public class StudyTopicItem
    {
        public string objName;
        public bool isDefault;
        public float timeStudiedTotalSeconds;
    
        public StudyTopicItem(string name, bool isDefault, float timeStudiedTotalSeconds)
        {
            this.objName = name;
            this.isDefault = isDefault;
            this.timeStudiedTotalSeconds = this.timeStudiedTotalSeconds;
        }
    }
    
    #endregion
}
