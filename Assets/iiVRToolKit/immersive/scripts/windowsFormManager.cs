using UnityEngine;

using System.Runtime.InteropServices;

public class windowsFormManager : MonoBehaviour
{

    [DllImport("user32.dll")]
	private static extern System.IntPtr GetActiveWindow();

	[DllImport("user32.dll", SetLastError=true)]
	public static extern System.IntPtr SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    
    const short SWP_NOSIZE = 1;
	const short SWP_NOZORDER = 0X4;
	const int SWP_SHOWWINDOW = 0x0040;

	System.IntPtr _activeWindowPtr ;

	int _windowsPosX = 0;
	int _windowsPosY = 0;
	int _windowsSizeX = 750;
	int _windowsSizeY = 500;

	bool _updated = false ;
	
	// Update is called once per frame
	void Update () 
	{
		int processId = iiVRUnityInterface.getProcessId ();
		iiVRUnityInterface.getViewCoordinates ((uint)processId ,out _windowsPosX,out _windowsPosY,out _windowsSizeX,out _windowsSizeY);
        //Debug.Log("ViewCoordinates " +  processId + " " + _windowsPosX + " " + _windowsPosY + " " + _windowsSizeX + " " + _windowsSizeY);
		if(_activeWindowPtr == System.IntPtr.Zero)
		{
			System.IntPtr ptr = GetActiveWindow();
			string Text = ptr.ToString();
			if(Text != "0")
			{
				_activeWindowPtr = ptr ;
			}
		}
		else
		{
			if(_updated == false)
			{
                SetWindowPos(_activeWindowPtr,System.IntPtr.Zero,_windowsPosX,_windowsPosY,_windowsSizeX,_windowsSizeY,SWP_SHOWWINDOW);

				Screen.SetResolution (_windowsSizeX,_windowsSizeY,false);
				_updated = true;
			}
		}
	}
}
