#pragma once
#include "PODImportsLight.h"
#include "CIsogenAssemblyLoader.h"
#include "tstring.h"

namespace IPI = Intergraph::PersonalISOGEN;

class LoadedPOD
{
private:
	AliasPOD::IPODPtr _Pod;
	IPI::CIsogenAssemblyLoader* _isogenAssemblyLoader;

public:
	LoadedPOD(tstring pod, tstring manifest);
	~LoadedPOD();

	AliasPOD::IPODPtr GetPod();
	IPI::CIsogenAssemblyLoader* GetIsogenAssemblyLoader();

};

