using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priority : MonoBehaviour
{
    public enum PriorityEnum { High = 3, Medium = 2, Low = 1, Zero = 0 }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    static public string GetPriorityLabel(PriorityEnum priority)
    {
        switch (priority)
        {
            case PriorityEnum.High:
                return "Pouso de emergência (prioridade 4)";
            case PriorityEnum.Medium:
                return "Conexão rápida (prioridade 3)";
            case PriorityEnum.Low:
                return "Permissão para decolar (prioridade 2)";
            case PriorityEnum.Zero:
                return "Permissão para decolar (prioridade 1)";
            default:
                return "";
        }
    }

   static public PriorityEnum GetFromInt(int num)
    {
        switch (num)
        {
            case 3:
                return PriorityEnum.High;
            case 2:
                return PriorityEnum.Medium;
            case 1:
                return PriorityEnum.Low;
            case 0:
                return PriorityEnum.Zero;
            default:
                return 0;
        }
    }
}
