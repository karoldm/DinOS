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
          "Bem-vindo ao local onde tudo come�a. Aqui � a �rea administrativa do aeroporto, onde os clientes agendam seus voos, gerando os planos de voo para a torre de controle. Esses planos precisam ser armazenados corretamente para que possam ser recuperados quando necess�rio.",
          "Para otimizar essa organiza��o, o aeroporto utiliza dois espa�os distintos:\nSagu�o (�rea de Swap) � Um local de acesso r�pido, ideal para planos de voo de alta prioridade.\nArmaz�m (Mem�ria Secund�ria / Disco R�gido) � Um espa�o maior, por�m de recupera��o mais lenta, onde ficam armazenados planos de voo de baixa prioridade.",
          "Para iniciar um atendimento, basta clicar no Dino. Em seguida ser� exibido um Dialog com as informa��es necess�rias (se a solicita��o � para armazenar ou recuperar um plano de voo e qual a sua prioridade).",
          "Para escrever um arquivo, clique na �rea desejada (sagu�o ou armaz�m).\nPara ler um plano de voo, clique nele e depois no cliente.\nPara mover um plano de voo, clique nele e depois na �rea desejada.\n",
          "Cuidado para n�o tentar armazenar nas �reas quando a opera��o for de leitura e vice-versa.\nSe atente tamb�m a prioridade do plano de voo solicitado, n�o tente recuperar um plano de voo de prioriadde baixa quando for solicitado um de prioridade alta, por exemplo",
          "Vale ressaltar que os clientes esperam por um tempo determinado para serem atendidos. Caso o tempo acabe, o cliente ir� embora e voc� perder� pontos.",
          "Por isso, guarde os planos de voos com cuidado, recuperar do sagu�o � bem mais r�pido do que recuperar do armaz�m, logo planos de alta prioridade devem ficar l�!",
          "Boa sorte!"
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] awardTexts =
        {
        "Conquista desbloqueada: Organiza��o � tudo! Parab�ns, voc� finalizou com gl�ria o atendimento de todos os nossos clientes! Voc� conseguiu manter tudo organizado por aqui e n�o causou nenhum transtorno.",
        "Isso n�o te lembra algo? Apesar de parecer mais complexo, um sistema de mem�ria secund�ria em um Sistema Computacional funciona de forma muito semelhante, armazenando os seus arquivos em se��es espec�ficas do disco, e fornecendo uma tabela virtual para que outras partes do sistema consigam acessar esses dados sem se preocupar com a implementa��o f�sica e interna da mem�ria secund�ria."
        };
    private LinkedList<string> awardDialog = new LinkedList<string>(awardTexts);

    private static string[] NoneTexts =
        {
            "As coisas parecem meio fora de ordem por aqui...",
        };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);


    private LinkedList<string> feedbackDialog = new LinkedList<string>();

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

    public void ShowFeedbackDialog(ControllerLevelFour.ErrorType errorType)
    {
        switch (errorType)
        {
            case ControllerLevelFour.ErrorType.TIMEOUT:
                feedbackDialog.AddLast("Tempo esgotado!\r\nO cliente esperou demais para receber o plano de voo, e ele n�o p�de ser lido a tempo. Lembre-se: planos de voo de alta prioridade devem ser mantidos no sagu�o (swap) para acesso r�pido, enquanto os de baixa prioridade podem ser armazenados no armaz�m (mem�ria secund�ria). Organize os planos de voo de acordo com suas prioridades para evitar atrasos cr�ticos!");
                break;
            case ControllerLevelFour.ErrorType.DIFFERENT_PRIORITY:
                feedbackDialog.AddLast("Prioridade incorreta!\r\nO cliente solicitou um plano de voo de prioridade diferente da que voc� serviu. Certifique-se de verificar a prioridade do plano antes de atend�-la!");
                break;
            case ControllerLevelFour.ErrorType.READ_WHEN_MUST_WRITE:
                feedbackDialog.AddLast("Ops! Parece que houve uma confus�o na opera��o!\r\nO cliente solicitou a escrita de um plano de voo, mas voc� tentou ler. Lembre-se: quando a torre pede para escrever, o plano de voo precisa ser armazenado corretamente no sagu�o (swap) ou no armaz�m (mem�ria secund�ria), dependendo da prioridade.");
                break;
            case ControllerLevelFour.ErrorType.WRITE_WHEN_MUST_READ:
                feedbackDialog.AddLast("Erro na opera��o!\r\nO cliente solicitou a leitura de um plano de voo, mas voc� tentou escrever. Quando a torre pede para ler, o objetivo � recuperar o plano de voo rapidamente, especialmente se for de alta prioridade. Certifique-se de identificar corretamente se a opera��o � de leitura ou escrita antes de agir!");
                break;
        }

        this.currentDialog = this.feedbackDialog;
        this.currentNode = this.currentDialog.First;
        this.nextText();
        this.show();
        feedbackDialog.Clear();
    }
}
