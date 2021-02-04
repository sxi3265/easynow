#pragma once

class NativeLibrary
{
public:
	const char * getPlatformABI();
	NativeLibrary();
	~NativeLibrary();
};

