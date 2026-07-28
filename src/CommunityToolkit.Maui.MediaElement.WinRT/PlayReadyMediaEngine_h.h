/* Header file automatically generated from PlayReadyMediaEngine.idl */
/*
 * File built with Microsoft(R) MIDLRT Compiler Engine Version 10.00.0231 
 */

#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

/* verify that the <rpcsal.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCSAL_H_VERSION__
#define __REQUIRED_RPCSAL_H_VERSION__ 100
#endif

#include <rpc.h>
#include <rpcndr.h>

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include <windows.h>
#include <ole2.h>
#endif /*COM_NO_WINDOWS_H*/
#ifndef __PlayReadyMediaEngine_h_h__
#define __PlayReadyMediaEngine_h_h__
#ifndef __PlayReadyMediaEngine_h_p_h__
#define __PlayReadyMediaEngine_h_p_h__


#pragma once

// Ensure that the setting of the /ns_prefix command line switch is consistent for all headers.
// If you get an error from the compiler indicating "warning C4005: 'CHECK_NS_PREFIX_STATE': macro redefinition", this
// indicates that you have included two different headers with different settings for the /ns_prefix MIDL command line switch
#if !defined(DISABLE_NS_PREFIX_CHECKS)
#define CHECK_NS_PREFIX_STATE "always"
#endif // !defined(DISABLE_NS_PREFIX_CHECKS)


#pragma push_macro("MIDL_CONST_ID")
#undef MIDL_CONST_ID
#define MIDL_CONST_ID const __declspec(selectany)


//  API Contract Inclusion Definitions
#if !defined(SPECIFIC_API_CONTRACT_DEFINITIONS)
#if !defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION)
#define WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION 0x70000
#endif // defined(WINDOWS_APPLICATIONMODEL_CALLS_CALLSPHONECONTRACT_VERSION)

#if !defined(WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION)
#define WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION 0x40000
#endif // defined(WINDOWS_FOUNDATION_FOUNDATIONCONTRACT_VERSION)

#if !defined(WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION)
#define WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION 0x130000
#endif // defined(WINDOWS_FOUNDATION_UNIVERSALAPICONTRACT_VERSION)

#if !defined(WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION)
#define WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION 0x30000
#endif // defined(WINDOWS_NETWORKING_SOCKETS_CONTROLCHANNELTRIGGERCONTRACT_VERSION)

#if !defined(WINDOWS_PHONE_PHONECONTRACT_VERSION)
#define WINDOWS_PHONE_PHONECONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_PHONE_PHONECONTRACT_VERSION)

#if !defined(WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION)
#define WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_PHONE_PHONEINTERNALCONTRACT_VERSION)

#if !defined(WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION)
#define WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION 0x10000
#endif // defined(WINDOWS_UI_WEBUI_CORE_WEBUICOMMANDBARCONTRACT_VERSION)

#endif // defined(SPECIFIC_API_CONTRACT_DEFINITIONS)


// Header files for imported files
#include "Windows.Foundation.h"
// Importing Collections header
#include <windows.foundation.collections.h>

#if defined(__cplusplus) && !defined(CINTERFACE)
#if defined(__MIDL_USE_C_ENUM)
#define MIDL_ENUM enum
#else
#define MIDL_ENUM enum class
#endif
/* Forward Declarations */
#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IPlayReadyDrmConfiguration;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyDrmConfiguration

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineStateChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgs

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineStateChangedEventArgsFactory;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgsFactory

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineTimeChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgs

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineTimeChangedEventArgsFactory;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgsFactory

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineErrorEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgs

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineErrorEventArgsFactory;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgsFactory

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineSizeChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgs

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IMediaEngineSizeChangedEventArgsFactory;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgsFactory

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IDrmLicenseAcquiredEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgs

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IDrmLicenseAcquiredEventArgsFactory;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgsFactory

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    interface IPlayReadyMediaEngine;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__

// Parameterized interface forward declarations (C++)

// Collection interface definitions

#ifndef DEF___FIKeyValuePair_2_HSTRING_HSTRING_USE
#define DEF___FIKeyValuePair_2_HSTRING_HSTRING_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("60310303-49c5-52e6-abc6-a9b36eccc716"))
IKeyValuePair<HSTRING,HSTRING> : IKeyValuePair_impl<HSTRING,HSTRING> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IKeyValuePair`2<String, String>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IKeyValuePair<HSTRING,HSTRING> __FIKeyValuePair_2_HSTRING_HSTRING_t;
#define __FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::__FIKeyValuePair_2_HSTRING_HSTRING_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>
//#define __FIKeyValuePair_2_HSTRING_HSTRING_t ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIKeyValuePair_2_HSTRING_HSTRING_USE */





#ifndef DEF___FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_USE
#define DEF___FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("05eb86f1-7140-5517-b88d-cbaebe57e6b1"))
IIterator<__FIKeyValuePair_2_HSTRING_HSTRING*> : IIterator_impl<__FIKeyValuePair_2_HSTRING_HSTRING*> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IIterator`1<Windows.Foundation.Collections.IKeyValuePair`2<String, String>>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IIterator<__FIKeyValuePair_2_HSTRING_HSTRING*> __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_t;
#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::__FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::IIterator<ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>*>
//#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_t ABI::Windows::Foundation::Collections::IIterator<ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_USE */





#ifndef DEF___FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_USE
#define DEF___FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("e9bdaaf0-cbf6-5c72-be90-29cbf3a1319b"))
IIterable<__FIKeyValuePair_2_HSTRING_HSTRING*> : IIterable_impl<__FIKeyValuePair_2_HSTRING_HSTRING*> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IIterable`1<Windows.Foundation.Collections.IKeyValuePair`2<String, String>>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IIterable<__FIKeyValuePair_2_HSTRING_HSTRING*> __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_t;
#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::__FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::IIterable<ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>*>
//#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_t ABI::Windows::Foundation::Collections::IIterable<ABI::Windows::Foundation::Collections::IKeyValuePair<HSTRING,HSTRING>*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_USE */




#ifndef DEF___FIMapView_2_HSTRING_HSTRING_USE
#define DEF___FIMapView_2_HSTRING_HSTRING_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("ac7f26f2-feb7-5b2a-8ac4-345bc62caede"))
IMapView<HSTRING,HSTRING> : IMapView_impl<HSTRING,HSTRING> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IMapView`2<String, String>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IMapView<HSTRING,HSTRING> __FIMapView_2_HSTRING_HSTRING_t;
#define __FIMapView_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::__FIMapView_2_HSTRING_HSTRING_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIMapView_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::IMapView<HSTRING,HSTRING>
//#define __FIMapView_2_HSTRING_HSTRING_t ABI::Windows::Foundation::Collections::IMapView<HSTRING,HSTRING>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIMapView_2_HSTRING_HSTRING_USE */




#ifndef DEF___FIMap_2_HSTRING_HSTRING_USE
#define DEF___FIMap_2_HSTRING_HSTRING_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("f6d1f700-49c2-52ae-8154-826f9908773c"))
IMap<HSTRING,HSTRING> : IMap_impl<HSTRING,HSTRING> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IMap`2<String, String>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IMap<HSTRING,HSTRING> __FIMap_2_HSTRING_HSTRING_t;
#define __FIMap_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::__FIMap_2_HSTRING_HSTRING_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIMap_2_HSTRING_HSTRING ABI::Windows::Foundation::Collections::IMap<HSTRING,HSTRING>
//#define __FIMap_2_HSTRING_HSTRING_t ABI::Windows::Foundation::Collections::IMap<HSTRING,HSTRING>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIMap_2_HSTRING_HSTRING_USE */




#ifndef DEF___FIIterator_1_byte_USE
#define DEF___FIIterator_1_byte_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("40556131-a2a1-5fab-aaee-5f35268ca26b"))
IIterator<::byte> : IIterator_impl<::byte> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IIterator`1<UInt8>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IIterator<::byte> __FIIterator_1_byte_t;
#define __FIIterator_1_byte ABI::Windows::Foundation::Collections::__FIIterator_1_byte_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIIterator_1_byte ABI::Windows::Foundation::Collections::IIterator<BYTE>
//#define __FIIterator_1_byte_t ABI::Windows::Foundation::Collections::IIterator<BYTE>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIIterator_1_byte_USE */




#ifndef DEF___FIIterable_1_byte_USE
#define DEF___FIIterable_1_byte_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("88318266-f3fd-50fc-8f08-b823a41b60c1"))
IIterable<::byte> : IIterable_impl<::byte> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IIterable`1<UInt8>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IIterable<::byte> __FIIterable_1_byte_t;
#define __FIIterable_1_byte ABI::Windows::Foundation::Collections::__FIIterable_1_byte_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIIterable_1_byte ABI::Windows::Foundation::Collections::IIterable<BYTE>
//#define __FIIterable_1_byte_t ABI::Windows::Foundation::Collections::IIterable<BYTE>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIIterable_1_byte_USE */




#ifndef DEF___FIVectorView_1_byte_USE
#define DEF___FIVectorView_1_byte_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("6d05fb29-7885-544e-9382-a1ad391a3fa4"))
IVectorView<::byte> : IVectorView_impl<::byte> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.Collections.IVectorView`1<UInt8>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef IVectorView<::byte> __FIVectorView_1_byte_t;
#define __FIVectorView_1_byte ABI::Windows::Foundation::Collections::__FIVectorView_1_byte_t
/* Collections */ } /* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FIVectorView_1_byte ABI::Windows::Foundation::Collections::IVectorView<BYTE>
//#define __FIVectorView_1_byte_t ABI::Windows::Foundation::Collections::IVectorView<BYTE>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FIVectorView_1_byte_USE */



namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class PlayReadyMediaEngine;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */




#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("be4e81ec-3897-5d2a-bcfb-7fec8951002e"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,IInspectable*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,IInspectable*> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Object>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,IInspectable*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,IInspectable*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,IInspectable*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_USE */




namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class MediaEngineErrorEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("3782bac1-baca-577f-8a3a-783258fa6451"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineErrorEventArgs*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineErrorEventArgs*, ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgs*>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineErrorEventArgs*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgs*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgs*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_USE */




namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class MediaEngineTimeChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("f9c949b2-9613-5632-bb5f-3870ed8c6f4f"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineTimeChangedEventArgs*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineTimeChangedEventArgs*, ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgs*>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineTimeChangedEventArgs*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgs*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgs*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_USE */





#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("1100e79e-036a-519b-ba68-ee3a8dcdd230"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,bool> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<bool, boolean>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Boolean>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,bool> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,boolean>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,boolean>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_USE */




namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class MediaEngineSizeChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("87e20b1f-82c0-52b9-b910-be69930c5b27"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineSizeChangedEventArgs*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineSizeChangedEventArgs*, ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgs*>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineSizeChangedEventArgs*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgs*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgs*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_USE */




namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class DrmLicenseAcquiredEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("f37c9a1e-2692-5893-b3f4-25331ab7dcff"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::DrmLicenseAcquiredEventArgs*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::DrmLicenseAcquiredEventArgs*, ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgs*>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::DrmLicenseAcquiredEventArgs*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgs*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgs*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_USE */




namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class MediaEngineStateChangedEventArgs;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_USE
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_USE
#if !defined(RO_NO_TEMPLATE_NAME)
namespace ABI { namespace Windows { namespace Foundation {
template <>
struct __declspec(uuid("66ac6990-648c-5a0f-a1d6-13e2679e8906"))
ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineStateChangedEventArgs*> : ITypedEventHandler_impl<ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*, ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*>,ABI::Windows::Foundation::Internal::AggregateType<ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineStateChangedEventArgs*, ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgs*>> 
{
    static const wchar_t* z_get_rc_name_impl() 
    {
        return L"Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs>"; 
    }
};
// Define a typedef for the parameterized interface specialization's mangled name.
// This allows code which uses the mangled name for the parameterized interface to access the
// correct parameterized interface specialization.
typedef ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::MediaEngineStateChangedEventArgs*> __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_t;
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs ABI::Windows::Foundation::__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_t
/* Foundation */ } /* Windows */ } /* ABI */ } 

////  Define an alias for the C version of the interface for compatibility purposes.
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgs*>
//#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_t ABI::Windows::Foundation::ITypedEventHandler<ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyMediaEngine*,ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgs*>
#endif // !defined(RO_NO_TEMPLATE_NAME)
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_USE */





namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    class PlayReadyDrmConfiguration;
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration:HRESULT get_LicenseServerUrl(String*);HRESULT put_LicenseServerUrl(String);HRESULT get_CustomHeaders(Windows.Foundation.Collections.IMap`2<String, String>**);HRESULT put_CustomHeaders(Windows.Foundation.Collections.IMap`2<String, String>*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IPlayReadyDrmConfiguration[] = L"CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("f047b6a3-e027-5b7f-b24b-a6572a45d0a8"), version, object, exclusiveto] */
                    MIDL_INTERFACE("f047b6a3-e027-5b7f-b24b-a6572a45d0a8")
                    IPlayReadyDrmConfiguration : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_LicenseServerUrl(
                            /* [out, retval] */HSTRING * value
                            ) = 0;
                        /* [propput] */virtual HRESULT STDMETHODCALLTYPE put_LicenseServerUrl(
                            /* [in] */HSTRING value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_CustomHeaders(
                            /* [out, retval] */__FIMap_2_HSTRING_HSTRING * * value
                            ) = 0;
                        /* [propput] */virtual HRESULT STDMETHODCALLTYPE put_CustomHeaders(
                            /* [in] */__FIMap_2_HSTRING_HSTRING * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IPlayReadyDrmConfiguration=__uuidof(IPlayReadyDrmConfiguration);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration[] = L"CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs:HRESULT get_OldState(Int32*);HRESULT get_NewState(Int32*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineStateChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("842ed4bc-5192-516b-ba39-d6ea690a5db5"), version, object, exclusiveto] */
                    MIDL_INTERFACE("842ed4bc-5192-516b-ba39-d6ea690a5db5")
                    IMediaEngineStateChangedEventArgs : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_OldState(
                            /* [out, retval] */int * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_NewState(
                            /* [out, retval] */int * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineStateChangedEventArgs=__uuidof(IMediaEngineStateChangedEventArgs);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory:HRESULT CreateInstance(Int32,Int32,CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineStateChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("ae97c6fb-745f-52f5-bad4-d9a2a31010e5"), version, object, exclusiveto] */
                    MIDL_INTERFACE("ae97c6fb-745f-52f5-bad4-d9a2a31010e5")
                    IMediaEngineStateChangedEventArgsFactory : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE CreateInstance(
                            /* [in] */int oldState,
                            /* [in] */int newState,
                            /* [retval, out] */ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineStateChangedEventArgs * * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineStateChangedEventArgsFactory=__uuidof(IMediaEngineStateChangedEventArgsFactory);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs:HRESULT get_PositionSeconds(Double*);HRESULT get_DurationSeconds(Double*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineTimeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("acbfc471-7914-5c50-b646-654b58658009"), version, object, exclusiveto] */
                    MIDL_INTERFACE("acbfc471-7914-5c50-b646-654b58658009")
                    IMediaEngineTimeChangedEventArgs : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_PositionSeconds(
                            /* [out, retval] */double * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_DurationSeconds(
                            /* [out, retval] */double * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineTimeChangedEventArgs=__uuidof(IMediaEngineTimeChangedEventArgs);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory:HRESULT CreateInstance(Double,Double,CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineTimeChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("dbacd624-9fd9-52e2-9692-97323eb5af9b"), version, object, exclusiveto] */
                    MIDL_INTERFACE("dbacd624-9fd9-52e2-9692-97323eb5af9b")
                    IMediaEngineTimeChangedEventArgsFactory : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE CreateInstance(
                            /* [in] */double positionSeconds,
                            /* [in] */double durationSeconds,
                            /* [retval, out] */ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineTimeChangedEventArgs * * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineTimeChangedEventArgsFactory=__uuidof(IMediaEngineTimeChangedEventArgsFactory);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs:HRESULT get_ErrorCode(Int32*);HRESULT get_ErrorMessage(String*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineErrorEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("6ea00545-d700-5174-b6b2-e64a6025dd86"), version, object, exclusiveto] */
                    MIDL_INTERFACE("6ea00545-d700-5174-b6b2-e64a6025dd86")
                    IMediaEngineErrorEventArgs : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_ErrorCode(
                            /* [out, retval] */int * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_ErrorMessage(
                            /* [out, retval] */HSTRING * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineErrorEventArgs=__uuidof(IMediaEngineErrorEventArgs);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory:HRESULT CreateInstance(Int32,String,CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineErrorEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("e7fcce98-c8c1-5808-97c1-7e150a094b12"), version, object, exclusiveto] */
                    MIDL_INTERFACE("e7fcce98-c8c1-5808-97c1-7e150a094b12")
                    IMediaEngineErrorEventArgsFactory : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE CreateInstance(
                            /* [in] */int errorCode,
                            /* [in] */HSTRING errorMessage,
                            /* [retval, out] */ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineErrorEventArgs * * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineErrorEventArgsFactory=__uuidof(IMediaEngineErrorEventArgsFactory);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs:HRESULT get_Width(UInt32*);HRESULT get_Height(UInt32*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineSizeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("4d50bab2-f7a8-5e6d-8e5b-9a44e71b5beb"), version, object, exclusiveto] */
                    MIDL_INTERFACE("4d50bab2-f7a8-5e6d-8e5b-9a44e71b5beb")
                    IMediaEngineSizeChangedEventArgs : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_Width(
                            /* [out, retval] */unsigned int * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_Height(
                            /* [out, retval] */unsigned int * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineSizeChangedEventArgs=__uuidof(IMediaEngineSizeChangedEventArgs);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory:HRESULT CreateInstance(UInt32,UInt32,CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineSizeChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("6d3cb95c-6ccb-513f-a4f3-1b1cbeb20a30"), version, object, exclusiveto] */
                    MIDL_INTERFACE("6d3cb95c-6ccb-513f-a4f3-1b1cbeb20a30")
                    IMediaEngineSizeChangedEventArgsFactory : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE CreateInstance(
                            /* [in] */unsigned int width,
                            /* [in] */unsigned int height,
                            /* [retval, out] */ABI::CommunityToolkit::Maui::Media::WinRT::IMediaEngineSizeChangedEventArgs * * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IMediaEngineSizeChangedEventArgsFactory=__uuidof(IMediaEngineSizeChangedEventArgsFactory);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs:HRESULT get_Success(Boolean*);HRESULT get_ErrorMessage(String*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IDrmLicenseAcquiredEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("844724e6-73b2-5bdd-8ce9-9572b002b855"), version, object, exclusiveto] */
                    MIDL_INTERFACE("844724e6-73b2-5bdd-8ce9-9572b002b855")
                    IDrmLicenseAcquiredEventArgs : public IInspectable
                    {
                    public:
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_Success(
                            /* [out, retval] */::boolean * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_ErrorMessage(
                            /* [out, retval] */HSTRING * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IDrmLicenseAcquiredEventArgs=__uuidof(IDrmLicenseAcquiredEventArgs);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory:HRESULT CreateInstance(Boolean,String,CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IDrmLicenseAcquiredEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("9aff49d5-4b2e-5ffb-baf6-b9dd7c88ce4e"), version, object, exclusiveto] */
                    MIDL_INTERFACE("9aff49d5-4b2e-5ffb-baf6-b9dd7c88ce4e")
                    IDrmLicenseAcquiredEventArgsFactory : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE CreateInstance(
                            /* [in] */::boolean success,
                            /* [in] */HSTRING errorMessage,
                            /* [retval, out] */ABI::CommunityToolkit::Maui::Media::WinRT::IDrmLicenseAcquiredEventArgs * * value
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IDrmLicenseAcquiredEventArgsFactory=__uuidof(IDrmLicenseAcquiredEventArgsFactory);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine:HRESULT Initialize(Int64);HRESULT SetSource(String,CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration*);HRESULT SetSourceFromManifestBytes(Windows.Foundation.Collections.IVectorView`1<UInt8>*,String,CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration*);HRESULT Play();HRESULT Pause();HRESULT Seek(Double);HRESULT SetVolume(Double);HRESULT SetMuted(Boolean);HRESULT SetPlaybackRate(Double);HRESULT SetLooping(Boolean);HRESULT SetStretch(Int32);HRESULT ResizeSwapChain(UInt32,UInt32);HRESULT Close();HRESULT get_CurrentPositionSeconds(Double*);HRESULT get_DurationSeconds(Double*);HRESULT get_NaturalVideoWidth(UInt32*);HRESULT get_NaturalVideoHeight(UInt32*);HRESULT get_IsBuffering(Boolean*);HRESULT add_MediaOpened(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Object>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaOpened(EventRegistrationToken);HRESULT add_MediaFailed(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaFailed(EventRegistrationToken);HRESULT add_MediaEnded(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Object>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaEnded(EventRegistrationToken);HRESULT add_TimeChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_TimeChanged(EventRegistrationToken);HRESULT add_BufferingChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Boolean>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_BufferingChanged(EventRegistrationToken);HRESULT add_SizeChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_SizeChanged(EventRegistrationToken);HRESULT add_LicenseAcquired(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_LicenseAcquired(EventRegistrationToken);HRESULT add_StateChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_StateChanged(EventRegistrationToken);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IPlayReadyMediaEngine[] = L"CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine";
namespace ABI {
    namespace CommunityToolkit {
        namespace Maui {
            namespace Media {
                namespace WinRT {
                    /* [uuid("3725974e-d708-5110-8b38-9824de4352a3"), version, object, exclusiveto] */
                    MIDL_INTERFACE("3725974e-d708-5110-8b38-9824de4352a3")
                    IPlayReadyMediaEngine : public IInspectable
                    {
                    public:
                        virtual HRESULT STDMETHODCALLTYPE Initialize(
                            /* [in] */__int64 swapChainPanelHandle
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetSource(
                            /* [in] */HSTRING url,
                            /* [in] */ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyDrmConfiguration * drmConfig
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetSourceFromManifestBytes(
                            /* [in] */__FIVectorView_1_byte * manifestData,
                            /* [in] */HSTRING originalUrl,
                            /* [in] */ABI::CommunityToolkit::Maui::Media::WinRT::IPlayReadyDrmConfiguration * drmConfig
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE Play(void) = 0;
                        virtual HRESULT STDMETHODCALLTYPE Pause(void) = 0;
                        virtual HRESULT STDMETHODCALLTYPE Seek(
                            /* [in] */double positionSeconds
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetVolume(
                            /* [in] */double volume
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetMuted(
                            /* [in] */::boolean muted
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetPlaybackRate(
                            /* [in] */double rate
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetLooping(
                            /* [in] */::boolean looping
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE SetStretch(
                            /* [in] */int stretchMode
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE ResizeSwapChain(
                            /* [in] */unsigned int width,
                            /* [in] */unsigned int height
                            ) = 0;
                        virtual HRESULT STDMETHODCALLTYPE Close(void) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_CurrentPositionSeconds(
                            /* [out, retval] */double * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_DurationSeconds(
                            /* [out, retval] */double * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_NaturalVideoWidth(
                            /* [out, retval] */unsigned int * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_NaturalVideoHeight(
                            /* [out, retval] */unsigned int * value
                            ) = 0;
                        /* [propget] */virtual HRESULT STDMETHODCALLTYPE get_IsBuffering(
                            /* [out, retval] */::boolean * value
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_MediaOpened(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_MediaOpened(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_MediaFailed(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_MediaFailed(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_MediaEnded(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_MediaEnded(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_TimeChanged(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_TimeChanged(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_BufferingChanged(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_BufferingChanged(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_SizeChanged(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_SizeChanged(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_LicenseAcquired(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_LicenseAcquired(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        /* [eventadd] */virtual HRESULT STDMETHODCALLTYPE add_StateChanged(
                            /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * handler,
                            /* [retval, out] */EventRegistrationToken * token
                            ) = 0;
                        /* [eventremove] */virtual HRESULT STDMETHODCALLTYPE remove_StateChanged(
                            /* [in] */EventRegistrationToken token
                            ) = 0;
                        
                    };

                    MIDL_CONST_ID IID & IID_IPlayReadyMediaEngine=__uuidof(IPlayReadyMediaEngine);
                    
                } /* WinRT */
            } /* Media */
        } /* Maui */
    } /* CommunityToolkit */
} /* ABI */

EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine[] = L"CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine";
#endif


#else // !defined(__cplusplus)
/* Forward Declarations */
#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine;

#endif // ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__

// Parameterized interface forward declarations (C)

// Collection interface definitions
#if !defined(____FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__)
#define ____FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__

typedef interface __FIKeyValuePair_2_HSTRING_HSTRING __FIKeyValuePair_2_HSTRING_HSTRING;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIKeyValuePair_2_HSTRING_HSTRING;

typedef struct __FIKeyValuePair_2_HSTRING_HSTRINGVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);

    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This);
    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This,
            /* [out] */ __RPC__out ULONG *iidCount,
            /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);
    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);
    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out HSTRING *key);
    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Value )(__RPC__in __FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__deref_out_opt HSTRING *value);
    END_INTERFACE
} __FIKeyValuePair_2_HSTRING_HSTRINGVtbl;

interface __FIKeyValuePair_2_HSTRING_HSTRING
{
    CONST_VTBL struct __FIKeyValuePair_2_HSTRING_HSTRINGVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FIKeyValuePair_2_HSTRING_HSTRING_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIKeyValuePair_2_HSTRING_HSTRING_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIKeyValuePair_2_HSTRING_HSTRING_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIKeyValuePair_2_HSTRING_HSTRING_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIKeyValuePair_2_HSTRING_HSTRING_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIKeyValuePair_2_HSTRING_HSTRING_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIKeyValuePair_2_HSTRING_HSTRING_get_Key(This,key)	\
    ( (This)->lpVtbl -> get_Key(This,key) ) 

#define __FIKeyValuePair_2_HSTRING_HSTRING_get_Value(This,value)	\
    ( (This)->lpVtbl -> get_Value(This,value) ) 
#endif /* COBJMACROS */


#endif // ____FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__



#if !defined(____FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__)
#define ____FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__

typedef interface __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING;

typedef struct __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl
{
    BEGIN_INTERFACE

    HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
        __RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This);
    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This,
        /* [out] */ __RPC__out ULONG *iidCount,
        /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);
    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Current )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out __FIKeyValuePair_2_HSTRING_HSTRING * *current);
    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_HasCurrent )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out boolean *hasCurrent);
    HRESULT ( STDMETHODCALLTYPE *MoveNext )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out boolean *hasCurrent);
    HRESULT ( STDMETHODCALLTYPE *GetMany )(__RPC__in __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING * This,
        /* [in] */ unsigned int capacity,
        /* [size_is][length_is][out] */ __RPC__out_ecount_part(capacity, *actual) __FIKeyValuePair_2_HSTRING_HSTRING * *items,
        /* [retval][out] */ __RPC__out unsigned int *actual);

    END_INTERFACE
} __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl;

interface __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING
{
    CONST_VTBL struct __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl *lpVtbl;
};



#ifdef COBJMACROS


#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_get_Current(This,current)	\
    ( (This)->lpVtbl -> get_Current(This,current) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_get_HasCurrent(This,hasCurrent)	\
    ( (This)->lpVtbl -> get_HasCurrent(This,hasCurrent) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_MoveNext(This,hasCurrent)	\
    ( (This)->lpVtbl -> MoveNext(This,hasCurrent) ) 

#define __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_GetMany(This,capacity,items,actual)	\
    ( (This)->lpVtbl -> GetMany(This,capacity,items,actual) ) 

#endif /* COBJMACROS */


#endif // ____FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__



#if !defined(____FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__)
#define ____FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__

typedef interface __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING;

typedef  struct __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl
{
    BEGIN_INTERFACE

    HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
        __RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);

    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This);

    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This);

    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This,
                                           /* [out] */ __RPC__out ULONG *iidCount,
                                           /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);

    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    HRESULT ( STDMETHODCALLTYPE *First )(__RPC__in __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__deref_out_opt __FIIterator_1___FIKeyValuePair_2_HSTRING_HSTRING **first);

    END_INTERFACE
} __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl;

interface __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING
{
    CONST_VTBL struct __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRINGVtbl *lpVtbl;
};

#ifdef COBJMACROS

#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_First(This,first)	\
    ( (This)->lpVtbl -> First(This,first) ) 

#endif /* COBJMACROS */


#endif // ____FIIterable_1___FIKeyValuePair_2_HSTRING_HSTRING_INTERFACE_DEFINED__


#if !defined(____FIMapView_2_HSTRING_HSTRING_INTERFACE_DEFINED__)
#define ____FIMapView_2_HSTRING_HSTRING_INTERFACE_DEFINED__

typedef interface __FIMapView_2_HSTRING_HSTRING __FIMapView_2_HSTRING_HSTRING;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIMapView_2_HSTRING_HSTRING;

typedef struct __FIMapView_2_HSTRING_HSTRINGVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);

    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This);

    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This);

    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This,
        /* [out] */ __RPC__out ULONG *iidCount,
        /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);
    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This,/* [out] */ __RPC__out TrustLevel *trustLevel);

    HRESULT ( STDMETHODCALLTYPE *Lookup )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in HSTRING key,
        /* [retval][out] */ __RPC__deref_out_opt HSTRING *value);
    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Size )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out unsigned int *size);
    HRESULT ( STDMETHODCALLTYPE *HasKey )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This, /* [in] */ __RPC__in HSTRING key, /* [retval][out] */ __RPC__out boolean *found);
    HRESULT ( STDMETHODCALLTYPE *Split )(__RPC__in __FIMapView_2_HSTRING_HSTRING * This,/* [out] */ __RPC__deref_out_opt __FIMapView_2_HSTRING_HSTRING **firstPartition,
        /* [out] */ __RPC__deref_out_opt __FIMapView_2_HSTRING_HSTRING **secondPartition);
    END_INTERFACE
} __FIMapView_2_HSTRING_HSTRINGVtbl;

interface __FIMapView_2_HSTRING_HSTRING
{
    CONST_VTBL struct __FIMapView_2_HSTRING_HSTRINGVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FIMapView_2_HSTRING_HSTRING_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 
#define __FIMapView_2_HSTRING_HSTRING_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 
#define __FIMapView_2_HSTRING_HSTRING_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 

#define __FIMapView_2_HSTRING_HSTRING_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 
#define __FIMapView_2_HSTRING_HSTRING_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 
#define __FIMapView_2_HSTRING_HSTRING_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 

#define __FIMapView_2_HSTRING_HSTRING_Lookup(This,key,value)	\
    ( (This)->lpVtbl -> Lookup(This,key,value) ) 
#define __FIMapView_2_HSTRING_HSTRING_get_Size(This,size)	\
    ( (This)->lpVtbl -> get_Size(This,size) ) 
#define __FIMapView_2_HSTRING_HSTRING_HasKey(This,key,found)	\
    ( (This)->lpVtbl -> HasKey(This,key,found) ) 
#define __FIMapView_2_HSTRING_HSTRING_Split(This,firstPartition,secondPartition)	\
    ( (This)->lpVtbl -> Split(This,firstPartition,secondPartition) ) 
#endif /* COBJMACROS */


#endif // ____FIMapView_2_HSTRING_HSTRING_INTERFACE_DEFINED__


#if !defined(____FIMap_2_HSTRING_HSTRING_INTERFACE_DEFINED__)
#define ____FIMap_2_HSTRING_HSTRING_INTERFACE_DEFINED__

typedef interface __FIMap_2_HSTRING_HSTRING __FIMap_2_HSTRING_HSTRING;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIMap_2_HSTRING_HSTRING;

typedef struct __FIMap_2_HSTRING_HSTRINGVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FIMap_2_HSTRING_HSTRING * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIMap_2_HSTRING_HSTRING * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIMap_2_HSTRING_HSTRING * This);

    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIMap_2_HSTRING_HSTRING * This,
        /* [out] */ __RPC__out ULONG *iidCount,
        /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);
    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIMap_2_HSTRING_HSTRING * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);
    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIMap_2_HSTRING_HSTRING * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    HRESULT ( STDMETHODCALLTYPE *Lookup )(__RPC__in __FIMap_2_HSTRING_HSTRING * This,
        /* [in] */ HSTRING key,
        /* [retval][out] */ __RPC__deref_out_opt HSTRING **value);
    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Size )(__RPC__in __FIMap_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__out unsigned int *size);
    HRESULT ( STDMETHODCALLTYPE *HasKey )(__RPC__in __FIMap_2_HSTRING_HSTRING * This, /* [in] */ HSTRING key, /* [retval][out] */ __RPC__out boolean *found);
    HRESULT ( STDMETHODCALLTYPE *GetView )(__RPC__in __FIMap_2_HSTRING_HSTRING * This, /* [retval][out] */ __RPC__deref_out_opt __FIMapView_2_HSTRING_HSTRING **view);
    HRESULT ( STDMETHODCALLTYPE *Insert )(__RPC__in __FIMap_2_HSTRING_HSTRING * This,
        /* [in] */ HSTRING key,
        /* [in] */ __RPC__in_opt HSTRING *value,
        /* [retval][out] */ __RPC__out boolean *replaced);
    HRESULT ( STDMETHODCALLTYPE *Remove )(__RPC__in __FIMap_2_HSTRING_HSTRING * This,/* [in] */ HSTRING key);
    HRESULT ( STDMETHODCALLTYPE *Clear )(__RPC__in __FIMap_2_HSTRING_HSTRING * This);
    END_INTERFACE
} __FIMap_2_HSTRING_HSTRINGVtbl;

interface __FIMap_2_HSTRING_HSTRING
{
    CONST_VTBL struct __FIMap_2_HSTRING_HSTRINGVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FIMap_2_HSTRING_HSTRING_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIMap_2_HSTRING_HSTRING_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIMap_2_HSTRING_HSTRING_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIMap_2_HSTRING_HSTRING_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIMap_2_HSTRING_HSTRING_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIMap_2_HSTRING_HSTRING_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIMap_2_HSTRING_HSTRING_Lookup(This,key,value)	\
    ( (This)->lpVtbl -> Lookup(This,key,value) ) 

#define __FIMap_2_HSTRING_HSTRING_get_Size(This,size)	\
    ( (This)->lpVtbl -> get_Size(This,size) ) 

#define __FIMap_2_HSTRING_HSTRING_HasKey(This,key,found)	\
    ( (This)->lpVtbl -> HasKey(This,key,found) ) 

#define __FIMap_2_HSTRING_HSTRING_GetView(This,view)	\
    ( (This)->lpVtbl -> GetView(This,view) ) 

#define __FIMap_2_HSTRING_HSTRING_Insert(This,key,value,replaced)	\
    ( (This)->lpVtbl -> Insert(This,key,value,replaced) ) 

#define __FIMap_2_HSTRING_HSTRING_Remove(This,key)	\
    ( (This)->lpVtbl -> Remove(This,key) ) 

#define __FIMap_2_HSTRING_HSTRING_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 
#endif /* COBJMACROS */



#endif // ____FIMap_2_HSTRING_HSTRING_INTERFACE_DEFINED__


#if !defined(____FIIterator_1_byte_INTERFACE_DEFINED__)
#define ____FIIterator_1_byte_INTERFACE_DEFINED__

typedef interface __FIIterator_1_byte __FIIterator_1_byte;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIIterator_1_byte;

typedef struct __FIIterator_1_byteVtbl
{
    BEGIN_INTERFACE

    HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
        __RPC__in __FIIterator_1_byte * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIIterator_1_byte * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIIterator_1_byte * This);
    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIIterator_1_byte * This,
        /* [out] */ __RPC__out ULONG *iidCount,
        /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIIterator_1_byte * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);
    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIIterator_1_byte * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Current )(__RPC__in __FIIterator_1_byte * This, /* [retval][out] */ __RPC__out byte *current);
    /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_HasCurrent )(__RPC__in __FIIterator_1_byte * This, /* [retval][out] */ __RPC__out boolean *hasCurrent);
    HRESULT ( STDMETHODCALLTYPE *MoveNext )(__RPC__in __FIIterator_1_byte * This, /* [retval][out] */ __RPC__out boolean *hasCurrent);
    HRESULT ( STDMETHODCALLTYPE *GetMany )(__RPC__in __FIIterator_1_byte * This,
        /* [in] */ unsigned int capacity,
        /* [size_is][length_is][out] */ __RPC__out_ecount_part(capacity, *actual) byte *items,
        /* [retval][out] */ __RPC__out unsigned int *actual);

    END_INTERFACE
} __FIIterator_1_byteVtbl;

interface __FIIterator_1_byte
{
    CONST_VTBL struct __FIIterator_1_byteVtbl *lpVtbl;
};



#ifdef COBJMACROS


#define __FIIterator_1_byte_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterator_1_byte_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterator_1_byte_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterator_1_byte_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterator_1_byte_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterator_1_byte_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterator_1_byte_get_Current(This,current)	\
    ( (This)->lpVtbl -> get_Current(This,current) ) 

#define __FIIterator_1_byte_get_HasCurrent(This,hasCurrent)	\
    ( (This)->lpVtbl -> get_HasCurrent(This,hasCurrent) ) 

#define __FIIterator_1_byte_MoveNext(This,hasCurrent)	\
    ( (This)->lpVtbl -> MoveNext(This,hasCurrent) ) 

#define __FIIterator_1_byte_GetMany(This,capacity,items,actual)	\
    ( (This)->lpVtbl -> GetMany(This,capacity,items,actual) ) 

#endif /* COBJMACROS */


#endif // ____FIIterator_1_byte_INTERFACE_DEFINED__


#if !defined(____FIIterable_1_byte_INTERFACE_DEFINED__)
#define ____FIIterable_1_byte_INTERFACE_DEFINED__

typedef interface __FIIterable_1_byte __FIIterable_1_byte;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIIterable_1_byte;

typedef  struct __FIIterable_1_byteVtbl
{
    BEGIN_INTERFACE

    HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
        __RPC__in __FIIterable_1_byte * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);

    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FIIterable_1_byte * This);

    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FIIterable_1_byte * This);

    HRESULT ( STDMETHODCALLTYPE *GetIids )(__RPC__in __FIIterable_1_byte * This,
                                           /* [out] */ __RPC__out ULONG *iidCount,
                                           /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(__RPC__in __FIIterable_1_byte * This, /* [out] */ __RPC__deref_out_opt HSTRING *className);

    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(__RPC__in __FIIterable_1_byte * This, /* [out] */ __RPC__out TrustLevel *trustLevel);

    HRESULT ( STDMETHODCALLTYPE *First )(__RPC__in __FIIterable_1_byte * This, /* [retval][out] */ __RPC__deref_out_opt __FIIterator_1_byte **first);

    END_INTERFACE
} __FIIterable_1_byteVtbl;

interface __FIIterable_1_byte
{
    CONST_VTBL struct __FIIterable_1_byteVtbl *lpVtbl;
};

#ifdef COBJMACROS

#define __FIIterable_1_byte_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterable_1_byte_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterable_1_byte_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterable_1_byte_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterable_1_byte_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterable_1_byte_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterable_1_byte_First(This,first)	\
    ( (This)->lpVtbl -> First(This,first) ) 

#endif /* COBJMACROS */


#endif // ____FIIterable_1_byte_INTERFACE_DEFINED__


#if !defined(____FIVectorView_1_byte_INTERFACE_DEFINED__)
#define ____FIVectorView_1_byte_INTERFACE_DEFINED__

typedef interface __FIVectorView_1_byte __FIVectorView_1_byte;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FIVectorView_1_byte;

typedef struct __FIVectorView_1_byteVtbl
{
    BEGIN_INTERFACE

    HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
        __RPC__in __FIVectorView_1_byte * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);

    ULONG ( STDMETHODCALLTYPE *AddRef )( __RPC__in __FIVectorView_1_byte * This);

    ULONG ( STDMETHODCALLTYPE *Release )( __RPC__in __FIVectorView_1_byte * This);

    HRESULT ( STDMETHODCALLTYPE *GetIids )( __RPC__in __FIVectorView_1_byte * This,
                                            /* [out] */ __RPC__out ULONG *iidCount,
                                            /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids);

    HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
        __RPC__in __FIVectorView_1_byte * This,
            /* [out] */ __RPC__deref_out_opt HSTRING *className);

    HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
        __RPC__in __FIVectorView_1_byte * This,
            /* [out] */ __RPC__out TrustLevel *trustLevel);

    HRESULT ( STDMETHODCALLTYPE *GetAt )( 
                                         __RPC__in __FIVectorView_1_byte * This,
                                         /* [in] */ unsigned int index,
                                         /* [retval][out] */ __RPC__out byte *item);

        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Size )( 
            __RPC__in __FIVectorView_1_byte * This,
            /* [retval][out] */ __RPC__out unsigned int *size);

        HRESULT ( STDMETHODCALLTYPE *IndexOf )( 
                                               __RPC__in __FIVectorView_1_byte * This,
            /* [in] */ byte item,
            /* [out] */ __RPC__out unsigned int *index,
            /* [retval][out] */ __RPC__out boolean *found);

        HRESULT ( STDMETHODCALLTYPE *GetMany )( 
                                               __RPC__in __FIVectorView_1_byte * This,
            /* [in] */ unsigned int startIndex,
            /* [in] */ unsigned int capacity,
            /* [size_is][length_is][out] */ __RPC__out_ecount_part(capacity, *actual) byte *items,
            /* [retval][out] */ __RPC__out unsigned int *actual);

        END_INTERFACE
} __FIVectorView_1_byteVtbl;

interface __FIVectorView_1_byte
{
    CONST_VTBL struct __FIVectorView_1_byteVtbl *lpVtbl;
};



#ifdef COBJMACROS


#define __FIVectorView_1_byte_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIVectorView_1_byte_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIVectorView_1_byte_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIVectorView_1_byte_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIVectorView_1_byte_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIVectorView_1_byte_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIVectorView_1_byte_GetAt(This,index,item)	\
    ( (This)->lpVtbl -> GetAt(This,index,item) ) 

#define __FIVectorView_1_byte_get_Size(This,size)	\
    ( (This)->lpVtbl -> get_Size(This,size) ) 

#define __FIVectorView_1_byte_IndexOf(This,item,index,found)	\
    ( (This)->lpVtbl -> IndexOf(This,item,index,found) ) 

#define __FIVectorView_1_byte_GetMany(This,startIndex,capacity,items,actual)	\
    ( (This)->lpVtbl -> GetMany(This,startIndex,capacity,items,actual) ) 

#endif /* COBJMACROS */



#endif // ____FIVectorView_1_byte_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectableVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt IInspectable * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectableVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectableVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgsVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgsVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__



#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_booleanVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt boolean e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_booleanVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_booleanVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgsVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgsVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__




#if !defined(____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__)
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__

typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs;

//  Declare the parameterized interface IID.
EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs;

typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This,
        /* [in] */ __RPC__in REFIID riid,
        /* [annotation][iid_is][out] */ 
        _COM_Outptr_  void **ppvObject);
    ULONG ( STDMETHODCALLTYPE *AddRef )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This);
    ULONG ( STDMETHODCALLTYPE *Release )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This);

    HRESULT ( STDMETHODCALLTYPE *Invoke )(__RPC__in __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * sender,/* [in] */ __RPC__in_opt __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * e);
    END_INTERFACE
} __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgsVtbl;

interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs
{
    CONST_VTBL struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_Invoke(This,sender,e)	\
    ( (This)->lpVtbl -> Invoke(This,sender,e) ) 
#endif /* COBJMACROS */



#endif // ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__





/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration:HRESULT get_LicenseServerUrl(String*);HRESULT put_LicenseServerUrl(String);HRESULT get_CustomHeaders(Windows.Foundation.Collections.IMap`2<String, String>**);HRESULT put_CustomHeaders(Windows.Foundation.Collections.IMap`2<String, String>*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IPlayReadyDrmConfiguration[] = L"CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration";
/* [uuid("f047b6a3-e027-5b7f-b24b-a6572a45d0a8"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_LicenseServerUrl )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
        /* [out, retval] */HSTRING * value
        );
    /* [propput] */HRESULT ( STDMETHODCALLTYPE *put_LicenseServerUrl )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
        /* [in] */HSTRING value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_CustomHeaders )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
        /* [out, retval] */__FIMap_2_HSTRING_HSTRING * * value
        );
    /* [propput] */HRESULT ( STDMETHODCALLTYPE *put_CustomHeaders )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
        /* [in] */__FIMap_2_HSTRING_HSTRING * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_get_LicenseServerUrl(This,value) \
    ( (This)->lpVtbl->get_LicenseServerUrl(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_put_LicenseServerUrl(This,value) \
    ( (This)->lpVtbl->put_LicenseServerUrl(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_get_CustomHeaders(This,value) \
    ( (This)->lpVtbl->get_CustomHeaders(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_put_CustomHeaders(This,value) \
    ( (This)->lpVtbl->put_CustomHeaders(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IPlayReadyDrmConfiguration ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_PlayReadyDrmConfiguration[] = L"CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs:HRESULT get_OldState(Int32*);HRESULT get_NewState(Int32*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineStateChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs";
/* [uuid("842ed4bc-5192-516b-ba39-d6ea690a5db5"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_OldState )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
        /* [out, retval] */int * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_NewState )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
        /* [out, retval] */int * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_get_OldState(This,value) \
    ( (This)->lpVtbl->get_OldState(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_get_NewState(This,value) \
    ( (This)->lpVtbl->get_NewState(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory:HRESULT CreateInstance(Int32,Int32,CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineStateChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgsFactory";
/* [uuid("ae97c6fb-745f-52f5-bad4-d9a2a31010e5"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateInstance )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
        /* [in] */int oldState,
        /* [in] */int newState,
        /* [retval, out] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_CreateInstance(This,oldState,newState,value) \
    ( (This)->lpVtbl->CreateInstance(This,oldState,newState,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineStateChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineStateChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs:HRESULT get_PositionSeconds(Double*);HRESULT get_DurationSeconds(Double*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineTimeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs";
/* [uuid("acbfc471-7914-5c50-b646-654b58658009"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_PositionSeconds )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
        /* [out, retval] */double * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_DurationSeconds )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
        /* [out, retval] */double * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_get_PositionSeconds(This,value) \
    ( (This)->lpVtbl->get_PositionSeconds(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_get_DurationSeconds(This,value) \
    ( (This)->lpVtbl->get_DurationSeconds(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory:HRESULT CreateInstance(Double,Double,CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineTimeChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgsFactory";
/* [uuid("dbacd624-9fd9-52e2-9692-97323eb5af9b"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateInstance )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
        /* [in] */double positionSeconds,
        /* [in] */double durationSeconds,
        /* [retval, out] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_CreateInstance(This,positionSeconds,durationSeconds,value) \
    ( (This)->lpVtbl->CreateInstance(This,positionSeconds,durationSeconds,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineTimeChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineTimeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs:HRESULT get_ErrorCode(Int32*);HRESULT get_ErrorMessage(String*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineErrorEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs";
/* [uuid("6ea00545-d700-5174-b6b2-e64a6025dd86"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_ErrorCode )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
        /* [out, retval] */int * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_ErrorMessage )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
        /* [out, retval] */HSTRING * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_get_ErrorCode(This,value) \
    ( (This)->lpVtbl->get_ErrorCode(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_get_ErrorMessage(This,value) \
    ( (This)->lpVtbl->get_ErrorMessage(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory:HRESULT CreateInstance(Int32,String,CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineErrorEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgsFactory";
/* [uuid("e7fcce98-c8c1-5808-97c1-7e150a094b12"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateInstance )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
        /* [in] */int errorCode,
        /* [in] */HSTRING errorMessage,
        /* [retval, out] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_CreateInstance(This,errorCode,errorMessage,value) \
    ( (This)->lpVtbl->CreateInstance(This,errorCode,errorMessage,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineErrorEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineErrorEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs:HRESULT get_Width(UInt32*);HRESULT get_Height(UInt32*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineSizeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs";
/* [uuid("4d50bab2-f7a8-5e6d-8e5b-9a44e71b5beb"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_Width )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
        /* [out, retval] */unsigned int * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_Height )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
        /* [out, retval] */unsigned int * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_get_Width(This,value) \
    ( (This)->lpVtbl->get_Width(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_get_Height(This,value) \
    ( (This)->lpVtbl->get_Height(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory:HRESULT CreateInstance(UInt32,UInt32,CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IMediaEngineSizeChangedEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgsFactory";
/* [uuid("6d3cb95c-6ccb-513f-a4f3-1b1cbeb20a30"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateInstance )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
        /* [in] */unsigned int width,
        /* [in] */unsigned int height,
        /* [retval, out] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_CreateInstance(This,width,height,value) \
    ( (This)->lpVtbl->CreateInstance(This,width,height,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IMediaEngineSizeChangedEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_MediaEngineSizeChangedEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs:HRESULT get_Success(Boolean*);HRESULT get_ErrorMessage(String*);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IDrmLicenseAcquiredEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs";
/* [uuid("844724e6-73b2-5bdd-8ce9-9572b002b855"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
/* [propget] */HRESULT ( STDMETHODCALLTYPE *get_Success )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
        /* [out, retval] */boolean * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_ErrorMessage )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
        /* [out, retval] */HSTRING * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_get_Success(This,value) \
    ( (This)->lpVtbl->get_Success(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_get_ErrorMessage(This,value) \
    ( (This)->lpVtbl->get_ErrorMessage(This,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__) */


/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory:HRESULT CreateInstance(Boolean,String,CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs**);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IDrmLicenseAcquiredEventArgsFactory[] = L"CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgsFactory";
/* [uuid("9aff49d5-4b2e-5ffb-baf6-b9dd7c88ce4e"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *CreateInstance )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
        /* [in] */boolean success,
        /* [in] */HSTRING errorMessage,
        /* [retval, out] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * * value
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_CreateInstance(This,success,errorMessage,value) \
    ( (This)->lpVtbl->CreateInstance(This,success,errorMessage,value) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IDrmLicenseAcquiredEventArgs ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_DrmLicenseAcquiredEventArgs[] = L"CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs";
#endif



/*
 *
 * Interface CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine
 *
 * Interface is a part of the implementation of type CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine
 *
 *
 * The IID for this interface was automatically generated by MIDLRT.
 *
 * Interface IID generation seed: CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine:HRESULT Initialize(Int64);HRESULT SetSource(String,CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration*);HRESULT SetSourceFromManifestBytes(Windows.Foundation.Collections.IVectorView`1<UInt8>*,String,CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration*);HRESULT Play();HRESULT Pause();HRESULT Seek(Double);HRESULT SetVolume(Double);HRESULT SetMuted(Boolean);HRESULT SetPlaybackRate(Double);HRESULT SetLooping(Boolean);HRESULT SetStretch(Int32);HRESULT ResizeSwapChain(UInt32,UInt32);HRESULT Close();HRESULT get_CurrentPositionSeconds(Double*);HRESULT get_DurationSeconds(Double*);HRESULT get_NaturalVideoWidth(UInt32*);HRESULT get_NaturalVideoHeight(UInt32*);HRESULT get_IsBuffering(Boolean*);HRESULT add_MediaOpened(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Object>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaOpened(EventRegistrationToken);HRESULT add_MediaFailed(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaFailed(EventRegistrationToken);HRESULT add_MediaEnded(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Object>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_MediaEnded(EventRegistrationToken);HRESULT add_TimeChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_TimeChanged(EventRegistrationToken);HRESULT add_BufferingChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, Boolean>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_BufferingChanged(EventRegistrationToken);HRESULT add_SizeChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_SizeChanged(EventRegistrationToken);HRESULT add_LicenseAcquired(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_LicenseAcquired(EventRegistrationToken);HRESULT add_StateChanged(Windows.Foundation.TypedEventHandler`2<CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine, CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs>*,Windows.Foundation.EventRegistrationToken*);HRESULT remove_StateChanged(EventRegistrationToken);
 *
 *
 */
#if !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__)
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_CommunityToolkit_Maui_Media_WinRT_IPlayReadyMediaEngine[] = L"CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine";
/* [uuid("3725974e-d708-5110-8b38-9824de4352a3"), version, object, exclusiveto] */
typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl
{
    BEGIN_INTERFACE
    HRESULT ( STDMETHODCALLTYPE *QueryInterface)(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
    /* [in] */ __RPC__in REFIID riid,
    /* [annotation][iid_is][out] */
    _COM_Outptr_  void **ppvObject
    );

ULONG ( STDMETHODCALLTYPE *AddRef )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This
    );

ULONG ( STDMETHODCALLTYPE *Release )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This
    );

HRESULT ( STDMETHODCALLTYPE *GetIids )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
    /* [out] */ __RPC__out ULONG *iidCount,
    /* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids
    );

HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
    /* [out] */ __RPC__deref_out_opt HSTRING *className
    );

HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )(
    __RPC__in __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
    /* [OUT ] */ __RPC__out TrustLevel *trustLevel
    );
HRESULT ( STDMETHODCALLTYPE *Initialize )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__int64 swapChainPanelHandle
        );
    HRESULT ( STDMETHODCALLTYPE *SetSource )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */HSTRING url,
        /* [in] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * drmConfig
        );
    HRESULT ( STDMETHODCALLTYPE *SetSourceFromManifestBytes )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FIVectorView_1_byte * manifestData,
        /* [in] */HSTRING originalUrl,
        /* [in] */__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * drmConfig
        );
    HRESULT ( STDMETHODCALLTYPE *Play )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This
        );
    HRESULT ( STDMETHODCALLTYPE *Pause )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This
        );
    HRESULT ( STDMETHODCALLTYPE *Seek )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */double positionSeconds
        );
    HRESULT ( STDMETHODCALLTYPE *SetVolume )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */double volume
        );
    HRESULT ( STDMETHODCALLTYPE *SetMuted )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */boolean muted
        );
    HRESULT ( STDMETHODCALLTYPE *SetPlaybackRate )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */double rate
        );
    HRESULT ( STDMETHODCALLTYPE *SetLooping )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */boolean looping
        );
    HRESULT ( STDMETHODCALLTYPE *SetStretch )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */int stretchMode
        );
    HRESULT ( STDMETHODCALLTYPE *ResizeSwapChain )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */unsigned int width,
        /* [in] */unsigned int height
        );
    HRESULT ( STDMETHODCALLTYPE *Close )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_CurrentPositionSeconds )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [out, retval] */double * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_DurationSeconds )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [out, retval] */double * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_NaturalVideoWidth )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [out, retval] */unsigned int * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_NaturalVideoHeight )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [out, retval] */unsigned int * value
        );
    /* [propget] */HRESULT ( STDMETHODCALLTYPE *get_IsBuffering )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [out, retval] */boolean * value
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_MediaOpened )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_MediaOpened )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_MediaFailed )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_MediaFailed )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_MediaEnded )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_MediaEnded )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_TimeChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_TimeChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_BufferingChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_BufferingChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_SizeChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_SizeChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_LicenseAcquired )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_LicenseAcquired )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    /* [eventadd] */HRESULT ( STDMETHODCALLTYPE *add_StateChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * handler,
        /* [retval, out] */EventRegistrationToken * token
        );
    /* [eventremove] */HRESULT ( STDMETHODCALLTYPE *remove_StateChanged )(
        __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
        /* [in] */EventRegistrationToken token
        );
    END_INTERFACE
    
} __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl;

interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine
{
    CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl *lpVtbl;
};

#ifdef COBJMACROS
#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_QueryInterface(This,riid,ppvObject) \
( (This)->lpVtbl->QueryInterface(This,riid,ppvObject) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_AddRef(This) \
        ( (This)->lpVtbl->AddRef(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Release(This) \
        ( (This)->lpVtbl->Release(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetIids(This,iidCount,iids) \
        ( (This)->lpVtbl->GetIids(This,iidCount,iids) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetRuntimeClassName(This,className) \
        ( (This)->lpVtbl->GetRuntimeClassName(This,className) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetTrustLevel(This,trustLevel) \
        ( (This)->lpVtbl->GetTrustLevel(This,trustLevel) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Initialize(This,swapChainPanelHandle) \
    ( (This)->lpVtbl->Initialize(This,swapChainPanelHandle) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetSource(This,url,drmConfig) \
    ( (This)->lpVtbl->SetSource(This,url,drmConfig) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetSourceFromManifestBytes(This,manifestData,originalUrl,drmConfig) \
    ( (This)->lpVtbl->SetSourceFromManifestBytes(This,manifestData,originalUrl,drmConfig) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Play(This) \
    ( (This)->lpVtbl->Play(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Pause(This) \
    ( (This)->lpVtbl->Pause(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Seek(This,positionSeconds) \
    ( (This)->lpVtbl->Seek(This,positionSeconds) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetVolume(This,volume) \
    ( (This)->lpVtbl->SetVolume(This,volume) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetMuted(This,muted) \
    ( (This)->lpVtbl->SetMuted(This,muted) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetPlaybackRate(This,rate) \
    ( (This)->lpVtbl->SetPlaybackRate(This,rate) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetLooping(This,looping) \
    ( (This)->lpVtbl->SetLooping(This,looping) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetStretch(This,stretchMode) \
    ( (This)->lpVtbl->SetStretch(This,stretchMode) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_ResizeSwapChain(This,width,height) \
    ( (This)->lpVtbl->ResizeSwapChain(This,width,height) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Close(This) \
    ( (This)->lpVtbl->Close(This) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_CurrentPositionSeconds(This,value) \
    ( (This)->lpVtbl->get_CurrentPositionSeconds(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_DurationSeconds(This,value) \
    ( (This)->lpVtbl->get_DurationSeconds(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_NaturalVideoWidth(This,value) \
    ( (This)->lpVtbl->get_NaturalVideoWidth(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_NaturalVideoHeight(This,value) \
    ( (This)->lpVtbl->get_NaturalVideoHeight(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_IsBuffering(This,value) \
    ( (This)->lpVtbl->get_IsBuffering(This,value) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaOpened(This,handler,token) \
    ( (This)->lpVtbl->add_MediaOpened(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaOpened(This,token) \
    ( (This)->lpVtbl->remove_MediaOpened(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaFailed(This,handler,token) \
    ( (This)->lpVtbl->add_MediaFailed(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaFailed(This,token) \
    ( (This)->lpVtbl->remove_MediaFailed(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaEnded(This,handler,token) \
    ( (This)->lpVtbl->add_MediaEnded(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaEnded(This,token) \
    ( (This)->lpVtbl->remove_MediaEnded(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_TimeChanged(This,handler,token) \
    ( (This)->lpVtbl->add_TimeChanged(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_TimeChanged(This,token) \
    ( (This)->lpVtbl->remove_TimeChanged(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_BufferingChanged(This,handler,token) \
    ( (This)->lpVtbl->add_BufferingChanged(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_BufferingChanged(This,token) \
    ( (This)->lpVtbl->remove_BufferingChanged(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_SizeChanged(This,handler,token) \
    ( (This)->lpVtbl->add_SizeChanged(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_SizeChanged(This,token) \
    ( (This)->lpVtbl->remove_SizeChanged(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_LicenseAcquired(This,handler,token) \
    ( (This)->lpVtbl->add_LicenseAcquired(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_LicenseAcquired(This,token) \
    ( (This)->lpVtbl->remove_LicenseAcquired(This,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_StateChanged(This,handler,token) \
    ( (This)->lpVtbl->add_StateChanged(This,handler,token) )

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_StateChanged(This,token) \
    ( (This)->lpVtbl->remove_StateChanged(This,token) )


#endif /* COBJMACROS */


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine;
#endif /* !defined(____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__) */


/*
 *
 * Class CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine
 *
 * RuntimeClass can be activated.
 *
 * Class implements the following interfaces:
 *    CommunityToolkit.Maui.Media.WinRT.IPlayReadyMediaEngine ** Default Interface **
 *
 * Class Threading Model:  Both Single and Multi Threaded Apartment
 *
 * Class Marshaling Behavior:  Agile - Class is agile
 *
 */

#ifndef RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine_DEFINED
#define RUNTIMECLASS_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_CommunityToolkit_Maui_Media_WinRT_PlayReadyMediaEngine[] = L"CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine";
#endif


#endif // defined(__cplusplus)
#pragma pop_macro("MIDL_CONST_ID")
#endif // __PlayReadyMediaEngine_h_p_h__

#endif // __PlayReadyMediaEngine_h_h__
