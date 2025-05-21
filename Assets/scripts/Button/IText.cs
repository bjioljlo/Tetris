using UnityEngine.UI;

public abstract class IText : IUI {
    Text m_text;

	public override void startInit()
	{
        m_text = gameObject.GetComponent<Text>();
        base.startInit();
	}

	public void SetText(string str)
    {
        m_text.text = str;
    }

}
