#pragma once
#include <map>
#include <set>
#include <atlstr.h>
#include <functional>

class CIsogenSharedData
{
public:
	struct PipelineAndSheet
	{
		CString pipelineReference;
		int sheetNumber;
	};

	typedef std::set<PipelineAndSheet, std::function<bool (const PipelineAndSheet&, const PipelineAndSheet&)>> PipelineAndSheetSet;
	typedef std::map<CString, PipelineAndSheetSet, std::function<bool (const CString&, const CString&)>> SheetsByDrawingMap;

	CIsogenSharedData(void);
	~CIsogenSharedData(void);
	std::map<CString, CString> ReportFiles;
	CString IsogenReportingLog;
	CString PodTransError;

	// This is the stuff that used to be in PISOGEN2PODTRANS.TMP
	bool keepPod;
	CString flsFile;
	CString isometricDirectory;
	CString projectName;
	CString styleName;
	CString podGraphicsStart;			// Note, these two could be removed and got from the FLS file in podtrans -
	int podGraphicsTimeout;				// would require GetPrePostProcessorTimeout() (in pisogen) reimplementing in podtrans
	bool pPodGraphicsStopOnError;
	CString personalIsogenVersion;
	CString isogenVersion;
	CString idfgenVersion;
	CString isogenMode;
	CString inputFile;
	CString podGraphicsMessageFile;
	CString podGraphicsTempDirectory;
	CString podGraphicsIniFile;

	std::map<CString, CString> PublishFiles;
	SheetsByDrawingMap SheetsByDrawing;

	int isogenTimeout;
};

