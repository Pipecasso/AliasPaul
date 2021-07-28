#pragma once
#include "Header.h"
#include "tstring.h"
#include "IAL.h"




class CPiqLiter
{
private:
	std::list<fsystem::path> _InputFiles;
	IPI::CIsogenAssemblyLoader* _loader;
	AliasImportExport::IImportExportPtr CreateImportExport();
	tstring _pcfPath;

	AliasPOD::IPODPtr MakePOD(TCHAR* szPODPath);
	
public:
	CPiqLiter(std::list<fsystem::path> & inputfiles, fsystem::path);
	void Go();
	tstring GetPcfPath();
	IPI::CIsogenAssemblyLoader* GetLoader();


};

