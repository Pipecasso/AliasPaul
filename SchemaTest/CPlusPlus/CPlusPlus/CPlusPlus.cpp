// CPlusPlus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "PODImports.h"
#include "CIsogenAssemblyLoaderCookie.h"
#include "Handshaker.h"
#include "AliasHandshakeStrings.h"  
using namespace Intergraph::PersonalISOGEN;

AliasImportExport::IImportExportPtr CreateImportExport(CIsogenAssemblyLoader *loader)
{
	CIsogenAssemblyLoaderCookie monster(loader);
	AliasImportExport::IImportExportPtr pImportExport(nullptr);
	HRESULT hresult;
	hresult = pImportExport.CreateInstance(__uuidof(AliasImportExport::ImportExport));
	return pImportExport;
}

AliasPOD::IPODPtr MakePOD(char* szPODPath)
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

int main()
{
    std::cout << "Hello World!\n";
	
	::CoInitializeEx(0, COINIT_APARTMENTTHREADED);
	char manifest[] = "I - Configure.exe.manifest";
	char root[] = "D:\\Benchmark\\2019R1HF2\\";

#ifdef  SCHEMA
	char manifest[] = "D:\\Benchmark\\IR - UX - Build\\IR - UX - Build\\I - Configure.exe.manifest";
	char root[] = "D:\\Benchmark\\IR - UX - Build\\IR - UX - Build";
#else


#endif //  SCHEMA


    CIsogenAssemblyLoader loader(manifest,root,root,true);
    CIsogenAssemblyLoaderCookie cookie(&loader, CActivationOptions::Both);

	char podpath[] = "D:\\PODPCFStore\\VerySimple.POD";
    AliasPOD::IPODPtr pPOD = MakePOD(podpath);
	AliasMaterialData::_MaterialDataPtr pMD(__uuidof(AliasMaterialData::MaterialData));
	
 
	pPOD = NULL;
	::CoUninitialize();
}




// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
