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
          "Bem-vindo ao local onde tudo come�a. Aqui � a �rea administrativa do aeroporto, onde nossos clientes agendam seus voos, gerando assim os planos de voo para a torre de controle.",
          "Aqui tamb�m � onde a torre de controle consegue armazenar e procurar pelos planos de voo dos clientes, para que possam ser executados.",
          "Para manter toda essa papelada organizada, nosso aeroporto faz uso de um sistema de arquivamento composto por estantes, prateleiras e um sistema computacional que gerencia os locais onde os planos s�o armazenados. Chamamos esse sistema de Tabela Virtual.",
          "Desse modo, toda vez que um voo � solicitado, gerado e armazenado em algumas estantes, o sistema Tabela Virtual deve ser atualizada com a localiza��o atual daquele arquivo. Esse processo � essencial para ajudar a recep��o a encontrar os planos mais facilmente e rapidamente quando a torre os solicitar.",
          "No momento, nossa recep��o est� lotada! Que tal ajudar nosso funcion�rio a atender todos? Para come�ar a atender um cliente, basta clicar nele, e ele lhe dir� qual opera��o deseja realizar e qual o identificador do voo.",
          "Voc� pode armazenar o plano de voo onde quiser, mas lembre-se de sempre manter a tabela atualizada quanto � sua posi��o, pois � a partir dela que a recep��o ir� recuperar o arquivo.",
          "Voc� pode associar as estantes �s partes da mem�ria secund�ria de um sistema computacional que s�o particionadas sem que os demais componentes precisam saber como, uma vez que o SO utiliza a tabela virtual para transitar os dados para a mem�ria secund�ria quando a RAM est� cheia. ",
          "Planos de voo n�o registrados na tabela ou registrados incorretamente resultam em uma perda de 3 pontos!",
          "Para ler ou escrever um arquivo, basta clicar no bot�o abaixo da estante que indica o seu identificador. Ao abrir a estante, voc� deve selecionar uma das quatro prateleiras e escolher entre escrever (armazenar arquivo) ou ler (recuperar arquivo).",
          "Boa sorte!"
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] awardTexts =
        {
        "Conquista desbloqueada: Organiza��o � tudo! Parab�ns, voc� finalizou com gl�ria o atendimento de todos os nossos clientes! Voc� conseguiu manter tudo organizado por aqui e n�o causou nenhum transtorno.",
        "Isso n�o te lembra algo? Apesar de parecer mais complexo, um sistema de mem�ria secund�ria em um Sistema Operacional funciona de forma muito semelhante, armazenando os seus arquivos em se��es espec�ficas do disco, e fornecendo uma tabela virtual para que outras partes do sistema consigam acessar esses dados sem se preocupar com a implementa��o f�sica e interna da mem�ria secund�ria."
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
