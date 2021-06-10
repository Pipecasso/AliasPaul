#include "LoadedPOD.h"
#include "CIsogenAssemblyLoaderCookie.h"
#include "Handshaker.h"
#include "AliasHandshakeStrings.h"

LoadedPOD::LoadedPOD(tstring pod, tstring manifest)
{
	_TCHAR szDrive[_MAX_DRIVE];
	_TCHAR szDir[_MAX_DIR];
	_TCHAR szName[_MAX_FNAME];
	_TCHAR szExt[_MAX_FNAME];
	_TCHAR szManifestFolder[_MAX_PATH];
	_TCHAR szCoreComponents[_MAX_PATH];
	_TCHAR szPersonalIsogen[MAX_PATH];
	_TCHAR szOda[MAX_PATH];
	_TCHAR szManifest[MAX_PATH];


	_tsplitpath_s(manifest.c_str(), szDrive, szDir, szName, szExt);
	_tmakepath_s(szManifestFolder, szDrive, szDir, _T(""), _T(""));
	_tcscpy_s(szCoreComponents, MAX_PATH, szManifestFolder);
	_tcscpy_s(szPersonalIsogen, MAX_PATH, szManifestFolder);
	_tcscat_s(szCoreComponents, MAX_PATH, _T("Core Components"));
	_tcscat_s(szPersonalIsogen, MAX_PATH, _T("Personal Isogen"));;
	_tcscpy_s(szOda, MAX_PATH, szPersonalIsogen);
	_tcscat_s(szOda, MAX_PATH, _T("\\ODA"));
	_tcscpy_s(szManifest, MAX_PATH, manifest.c_str());

	::CoInitialize(0);

	_isogenAssemblyLoader = new IPI::CIsogenAssemblyLoader(szManifest, szManifestFolder);
	_isogenAssemblyLoader->AddStringPath(szCoreComponents);
	_isogenAssemblyLoader->AddStringPath(szPersonalIsogen);
	_isogenAssemblyLoader->AddStringPath(szOda);


	IPI::CIsogenAssemblyLoaderCookie* monster = new IPI::CIsogenAssemblyLoaderCookie(_isogenAssemblyLoader);
	HRESULT hr =_Pod.CreateInstance(__uuidof(AliasPOD::POD));
	char* szHandshake = CreateHandshake(szDevId, szPodStaticKey);
	_bstr_t bstrHandshake(szHandshake);
	delete szHandshake;
	_Pod->Handshake = bstrHandshake;
	_Pod->Load((_bstr_t)pod.c_str());

	delete monster;

}

LoadedPOD::~LoadedPOD()
{
	_Pod = NULL;
	::CoUninitialize();
}

AliasPOD::IPODPtr LoadedPOD::GetPod()
{
	return _Pod;
}


IPI::CIsogenAssemblyLoader* LoadedPOD::GetIsogenAssemblyLoader()
{
	return _isogenAssemblyLoader;
}

