#pragma once
#include "tstring.h"
class CommandOptions
{
private:
	bool _InputPcf;
	tstring _Style;
	tstring _IsometricDirectory;
	tstring _Project;
	tstring _manifest;
	tstring _path;


private:
	void LoadCommandLineOption(TCHAR* szKeyword, TCHAR* szValue);
	int GetKeywordAndValue(TCHAR* szBuffer, TCHAR* szKeyword, TCHAR* szValue);
	void RemoveLeadingWhiteSpace(TCHAR* s);
	void RemoveTrailingWhiteSpace(TCHAR* s);
public:
	CommandOptions(int argc, TCHAR* argv[]);
	bool GetInputPCF();
	tstring GetStyle();
	tstring GetIsoDir();
	tstring GetProject();
	tstring GetManifest();
	tstring GetPath();
	

};

