#include "PiqLiter.h"
#include "Header.h"
#include <tchar.h>
#include "CIsogenAssemblyLoaderCookie.h"
#include "AliasHandshakeStrings.h"
#include "Handshaker.h"


CPiqLiter::CPiqLiter(std::list<fsystem::path>& inputfiles,fsystem::path manipath) : _InputFiles(inputfiles)
{
	// wchar_t* pmanifestfile = manipath.filename().string().c_str();
	 //char_t* pmanifestdir =  manipath.remove_filename().string().c_str();
	
	TCHAR szManifestFile[BUFSIZ];
	_tcscpy_s(szManifestFile, BUFSIZ, manipath.filename().wstring().c_str());
	TCHAR szManifestDir[BUFSIZ];
	_tcscpy_s(szManifestDir, BUFSIZ, manipath.parent_path().wstring().c_str());
	_loader = new IPI::CIsogenAssemblyLoader(szManifestFile, szManifestDir, szManifestDir, true);
}

void CPiqLiter::Go()
{
	AliasImportExport::IImportExportPtr pIE = CreateImportExport();
	
	
	//for (std::list<fsystem::path>::iterator it = _InputFiles.begin(); it != _InputFiles.end() ; it++)
	for (fsystem::path p : _InputFiles)
	{
		TCHAR szInputDir[BUFSIZ];
		TCHAR szInputPath[BUFSIZ];
		TCHAR szOutputPath[BUFSIZ];
		TCHAR szOutputPcf[BUFSIZ];
		
		_tcscpy_s(szInputPath, BUFSIZ, p.wstring().c_str());
		_tcscpy_s(szInputDir, BUFSIZ, p.parent_path().wstring().c_str());
		_tcscpy_s(szOutputPath, BUFSIZ, szInputDir);
		_tcscat_s(szOutputPath, BUFSIZ, _T("\\New\\"));
		_tcscat_s(szOutputPath, BUFSIZ, p.stem().wstring().c_str());
		_tcscpy_s(szOutputPcf, BUFSIZ, szOutputPath);
		_tcscat_s(szOutputPath, BUFSIZ, _T(".pod"));
		_tcscat_s(szOutputPcf, BUFSIZ, _T(".pcf"));

		pIE->InputType = AliasImportExport::eInputTypeIDF;
		pIE->InputName = (_bstr_t)szInputPath;
		pIE->OutputName = (_bstr_t)szOutputPath;
		pIE->OutputType = AliasImportExport::eOutputTypePOD;

		IPI::CIsogenAssemblyLoaderCookie monster(_loader);
		long lstatus = pIE->Execute();

		if (lstatus == 0)
		{
			AliasPOD::IPODPtr pPOD = MakePOD(szOutputPath);
			pIE->InputObject = pPOD;
			pIE->InputType = AliasImportExport::eInputTypePOD;
			pIE->InputName = _T("");
			pIE->OutputType = AliasImportExport::eOutputTypePCF;
			pIE->OutputName = (_bstr_t)szOutputPcf;
			lstatus = pIE->Execute();
		}
	}
	pIE = NULL;
	


}



AliasImportExport::IImportExportPtr CPiqLiter::CreateImportExport()
{
	IPI::CIsogenAssemblyLoaderCookie monster(_loader);
	AliasImportExport::IImportExportPtr pImportExport(nullptr);
	HRESULT hresult;
	hresult = pImportExport.CreateInstance(__uuidof(AliasImportExport::ImportExport));
	switch (hresult)
	{
	case S_OK:
		break;
	case REGDB_E_CLASSNOTREG:
		_tprintf(_T("Alias ImportExport not registered\n"));
		break;
	default:
		_tprintf(_T("Error 0x%x attempting to load Alias ImportExport\n"), hresult);
		break;
	}
	return pImportExport;
}

AliasPOD::IPODPtr CPiqLiter::MakePOD(TCHAR* szPODPath)
{
	AliasPOD::IPODPtr pPod(nullptr);
	HRESULT hresult = pPod.CreateInstance(__uuidof(AliasPOD::POD));

	char* szHandshake = CreateHandshake(szDevId, szPodStaticKey);
	_bstr_t bstrHandshake(szHandshake);
	delete szHandshake;
	pPod->Handshake = bstrHandshake;
	pPod->Load((_bstr_t)szPODPath);
	return pPod;
	

}



