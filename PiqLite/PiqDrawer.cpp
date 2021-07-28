#include "PiqDrawer.h"
#include "encrypt.h"
#include "tstringconversion.h"
#include <iostream>


void PiqDrawer::Initialise()
{
	_style->ReadStyle();
	_style->ExportStyle();

	if (_pIsogen == NULL) _pIsogen.CreateInstance(__uuidof(AliasPersonalIsogen::PersonalIsogen2));

	// Create the handshake
	long lday, lmonth, lyear, lhour, lmin;
	char szHandshake[LEN_ENCRYPT + 1];

	long lSecondsCount = _pIsogen->GetHandshakeTime(&lday, &lmonth, &lyear, &lhour, &lmin);
	int iHandshakeMode = AliasPersonalIsogen::ePersonalIsogenModeFULL;

	CreateEncrypt(iHandshakeMode, szHandshake, lSecondsCount);


	//Directory and project
	_pIsogen->RootDirectory = _bstr_t(_isometricdir->Name) + _T("\\");
	_pIsogen->Project = _project->Name;

	// Style - nb must have been exported
	_pIsogen->Style = _style->Name;

	//Set units, viewpoint, etc.
	//_pIsogen->DimensionUnits = Units;
	//_pIsogen->Viewpoint = 
	_pIsogen->WeightUnits = AliasPersonalIsogen::eWeightUnitsUSER;
	_pIsogen->DrawingFormat = AliasPersonalIsogen::eDrawingFormatUSER;
	_pIsogen->Tolerance = 0;
	_pIsogen->MessageLevel = AliasPersonalIsogen::eMessageLevelUSER;

	//Keep the POD?
	_pIsogen->KeepPOD = VARIANT_TRUE;

	

}

void PiqDrawer::Go(tstring pcf)
{
	//Input file and handshake
	Initialise();
	//Load and run isogen
	_pIsogen->LoadIsogen();
	_pIsogen->InputName = tstring2bstr(pcf);;
	int iStatus = _pIsogen->Execute();
	// To avoid recursive running issues
	_pIsogen->UnloadIsogen();
}

void PiqDrawer::BatchGo(std::list<fsystem::path>& inputfiles)
{
	Initialise();
	//Load and run isogen
	_pIsogen->LoadIsogen();
	for (fsystem::path p : inputfiles)
	{
		TCHAR szInputDir[BUFSIZ];
		TCHAR szInputPath[BUFSIZ];


		_tcscpy_s(szInputPath, BUFSIZ, p.wstring().c_str());
		_tcscpy_s(szInputDir, BUFSIZ, p.parent_path().wstring().c_str());

		_bstr_t bstrPath(szInputPath);
		std::wcout << _T("Lets Generate ") << (szInputPath) << std::endl;
		_pIsogen->InputName = bstrPath;
		int iStatus = _pIsogen->Execute();
	}
	_pIsogen->UnloadIsogen();
}
