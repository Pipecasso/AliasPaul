#include "IsogenSharedData.h"

CIsogenSharedData::CIsogenSharedData(void)
{
	auto CStringLess = [] (const CString& lhs, const CString &rhs)
	{
		return lhs.CompareNoCase(rhs) < 0;
	};

	SheetsByDrawing = SheetsByDrawingMap(CStringLess);

	ReportFiles.clear();
	IsogenReportingLog.Empty();
	PodTransError.Empty();
	keepPod = false;
	flsFile.Empty();
	isometricDirectory.Empty();
	projectName.Empty();
	styleName.Empty();
	podGraphicsStart.Empty();
	podGraphicsTimeout = 0;
	pPodGraphicsStopOnError = false;
	personalIsogenVersion.Empty();
	isogenVersion.Empty();
	idfgenVersion.Empty();
	isogenMode.Empty();
	inputFile.Empty();
	podGraphicsMessageFile.Empty();
	podGraphicsTempDirectory.Empty();
	podGraphicsIniFile.Empty();
	PublishFiles.clear();
	SheetsByDrawing.clear();
	isogenTimeout = 0;
}

CIsogenSharedData::~CIsogenSharedData(void)
{
}
