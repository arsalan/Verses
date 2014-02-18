#include "monotouch/main.h"


void monotouch_register_modules ()
{

}

void monotouch_register_assemblies ()
{
	monotouch_open_and_register ("monotouch.dll");
	monotouch_open_and_register ("SQLite.dll");
	monotouch_open_and_register ("MonoTouch.Dialog-1.dll");
	monotouch_open_and_register ("Localytics.dll");
	monotouch_open_and_register ("MTiRate.dll");

}

void monotouch_setup ()
{
	use_old_dynamic_registrar = TRUE;
	monotouch_assembly_name = "VersesiOS.exe";
	mono_use_llvm = FALSE;
	monotouch_log_level = 0;
	monotouch_new_refcount = FALSE;
	monotouch_sgen = TRUE;
	setenv ("MONO_GC_PARAMS", "nursery-size=512k", 1);
}

