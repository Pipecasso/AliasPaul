#pragma once
#include "Header.h"
#include "CIsogenAssemblyLoader.h"



namespace IPI = Intergraph::PersonalISOGEN;


class CPiqLiter
{
private:
	std::list<fsystem::path> _InputFiles;
	IPI::CIsogenAssemblyLoader* _loader;
	AliasImportExport::IImportExportPtr CreateImportExport();


	AliasPOD::IPODPtr MakePOD(TCHAR* szPODPath);
	
public:
	CPiqLiter(std::list<fsystem::path> & inputfiles, fsystem::path);
	void Go();


};

