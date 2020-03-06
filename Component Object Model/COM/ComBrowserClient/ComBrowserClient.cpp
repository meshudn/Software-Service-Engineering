// ComBrowserClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
#include <iostream>

#import "{Path to COMBrowserServer\bin\Debug}\ComBrowser.tlb" named_guids raw_interfaces_only


int _tmain(int argc, _TCHAR* argv[])
{
	CoInitialize(NULL);   

	ComBrowser::IBrowerPtr pBrowserPtr;	
	HRESULT hRes = 
		pBrowserPtr.CreateInstance(ComBrowser::CLSID_Browser);
	if (hRes == S_OK)
	{				
		CComBSTR url("http://www.chemnitz.de/");
		
		BSTR str;		
		BSTR url2 = url.Copy();
		pBrowserPtr->HttpGet(url2,&str);
		printf("HTTP-Anwort:\r\n %S\r\n", str);		
	}

	CoUninitialize ();
	system("Pause");
	return 0;
}

