#pragma once

#include "Header.h"
#include "tstring.h"
#include "IAL.h"

class PiqDrawer
{
private:
	AliasProjectManager::_IsometricDirPtr _isometricdir;
	AliasProjectManager::_ProjectPtr _project;
	AliasProjectManager::_StylePtr _style;
	AliasPersonalIsogen::IPersonalIsogen2Ptr _pIsogen;
	IPI::CIsogenAssemblyLoader* _loader;


	void Initialise();

public:
	PiqDrawer(IPI::CIsogenAssemblyLoader* loader,AliasProjectManager::_IsometricDirPtr isoDir, AliasProjectManager::_ProjectPtr project, AliasProjectManager::_StylePtr style) : _isometricdir(isoDir),_project(project),_style(style){}
	
	
	void Go(tstring pcf);
	void BatchGo(std::list<fsystem::path>& inputfiles);


};

