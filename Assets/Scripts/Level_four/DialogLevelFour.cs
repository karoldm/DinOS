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
          "Bem-vindo ao local onde tudo começa. Aqui é a área administrativa do aeroporto, onde os clientes agendam seus voos, gerando os planos de voo para a torre de controle. Esses planos precisam ser armazenados corretamente para que possam ser recuperados quando necessário.",
          "Para otimizar essa organização, o aeroporto utiliza dois espaços distintos:\n Saguão (Área de Swap) – Um local de acesso rápido, ideal para planos de voo de alta prioridade.\n Armazém (Memória Secundária / Disco Rígido) – Um espaço maior, porém de recuperação mais lenta, onde ficam armazenados planos de voo de baixa prioridade.",
          "Para iniciar um atendimento, basta clicar no Dino. Em seguida será exibido um Dialog com as informações necessárias (se a solicitação é para armazenar ou recuperar um plano de voo e qual a sua prioridade).",
          "Para escrever um arquivo, clique na área desejada (saguão ou armazém).\nPara ler um plano de voo, clique nele e depois no cliente.\nPara mover um plano de voo, clique nele e depois na área desejada.\n",
          "Cuidado para não tentar armazenar nas áreas quando a operação for de leitura e vice-versa.\nSe atente também a prioridade do plano de voo solicitado, não tente recuperar um plano de voo de prioriadde baixa quando for solicitado um de prioridade alta, por exemplo",
          "Vale ressaltar que os clientes esperam por um tempo determinado para serem atendidos. Caso o tempo acabe, o cliente irá embora e você perderá pontos.",
          "Por isso, guarde os planos de voos com cuidado, recuperar do saguão é bem mais rápido do que recuperar do armazém, logo planos de alta prioridade devem ficar lá!",
          "Boa sorte!"
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] awardTexts =
        {
        "Conquista desbloqueada: Organização é tudo! Parabéns, você finalizou com glória o atendimento de todos os nossos clientes! Você conseguiu manter tudo organizado por aqui e não causou nenhum transtorno.",
        "Isso não te lembra algo? Apesar de parecer mais complexo, um sistema de memória secundária em um Sistema Computacional funciona de forma muito semelhante, armazenando os seus arquivos em seções específicas do disco, e fornecendo uma tabela virtual para que outras partes do sistema consigam acessar esses dados sem se preocupar com a implementação física e interna da memória secundária."
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
