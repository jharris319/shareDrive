shareDrive
==========

A network drive mapping application written in C#.

#### Overview ####

shareDrive utilizes DLLImport from InteropServices to call methods from mpr.dll. A form is presented to the user requesting login credentials which are then used to invoke WNetAddConnection2. A sleeper form is called and remains open while the user is accessing the shared resources. Upon closing the sleeper window, WNetCancelConnection2 is invoked to disconnect the mapped network drives and shareDrive terminates.
