using UnityEngine;

namespace CodeMonkey
{

    /*
     * 
       Referências de ativos globais
     * Edite referências de ativos no CodeMonkey/Resources/CodeMonkeyAssets pré-fabricado
     * */
    public class Assets : MonoBehaviour
    {

        // Referência interna da instância
        private static Assets _i;

        // Referência da instância

        public static Assets i
        {
            get
            {
                // Se _i ainda não estiver inicializado, instancia o objeto CodeMonkeyAssets do diretório Resources e obtém o componente Assets
                if (_i == null) _i = (Instantiate(Resources.Load("CodeMonkeyAssets")) as GameObject).GetComponent<Assets>();
                return _i;
            }
        }


        // Todas as referências de ativos

        public Sprite s_White;//Esta é uma variável pública que armazena uma referência a um sprite branco. Isso permite que outros scripts acessem este sprite através da instância única da classe Assets.


    }

}
