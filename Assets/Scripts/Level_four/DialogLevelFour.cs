using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogLevelFour : MonoBehaviour
{
    public enum DialogType
    {
        award,
        none,
        intro,
    }

    private static string[] introTexts =
        {
           "Bem-vindo ao local aonde tudo começa. Aqui é a área administrativa do aeroporto e é aonde nossos clientes agendam seus voos, gerando assim os planos de voo para a torre de controle.",
           "Aqui também é aonde a torre de controle consegue armazenar e procurar pelos planos de voo dos clientes para que eles possam ser executados.",
            "Para manter toda essa papelada organizada nosso aeroporto faz uso de um sistema de arquivamento que é composto por estantes, prateleiras e um sistema computacional que gerencia os locais onde os planos são armazenados. Chamamos esse sistema de tabela virtual.",
            "Desse modo, toda vez que um voo é solicitado, gerado e armazenado em alguma estate, a tabela virtual deve ser atualizada com a localização atual daquele arquivo. Esse processo é essencial para ajudar a recepção a encontrar os planos mais fácil e rápido quando a torre solicitar eles."
            "No momento a nossa recepção está lotada! Que tal ajuda nosso funcionário a atender todos eles? Para começar a tender um cliente basta clicar nele, e ele lhe dirá qual operação ele deseja realizar e qual o identificador do voo."
            "Você pode armazenar o plano de voo aonde quiser, mas lembre-se de sempre manter a tabela atualizada quanto a sua posição, pois é a partir dela que a recepção irá recuperar o arquivo. Planos de voos não escritos na tabela ou escritos incorretamente geram uma perca de 3 pontos!"
            "Para ler ou escrever umarquivo, basta clicar no botão abaixo da estante que indica o seu identificador. Ao abrir a estante, você deve selecionar uma das quatro prateleiras, e escolher entre escrever (armazenar arquivo) ou ler (recuperar arquivo)."
            "Boa sorte!"
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] awardTexts =
        {
           "Parabéns, você finalizou com glória o atendimento de todos os nossos clientes! Você comseguiu manter tudo organizado por aqui e não causou nenhum transtorno.",
           "Conquista desbloqueada: Organização é tudo!"
        };
    private LinkedList<string> awardDialog = new LinkedList<string>(awardTexts);

    private static string[] NoneTexts =
        {
            "As coisas parecem meio fora de ordem por aqui...",
        };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    ControllerLevelFour controller;

    private static DialogLevelFour instance;
    public static DialogLevelFour Instance => instance;

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in the scene.");
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void nextText()
    {

        if (this.currentNode != null)
        {
            this.buildText();
            this.currentNode = this.currentNode.Next;
        }
        else
        {
            this.hidden();
        }
    }

    private void buildText()
    {
        if (this.currentNode != null && this.dialogText != null)
        {
            this.dialogText.text = this.currentNode.Value;
        }
    }

    private void show()
    {
        gameObject.SetActive(true);
    }

    private void hidden()
    {
        gameObject.SetActive(false);
    }

    public void showDialog(DialogType type)
    {
        switch (type)
        {
            case DialogType.award:
                this.currentDialog = this.awardDialog; break;
            case DialogType.intro:
                this.currentDialog = this.introDialog; break;
            default:
                this.currentDialog = this.NoneDialog; break;
        }

        if (this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
