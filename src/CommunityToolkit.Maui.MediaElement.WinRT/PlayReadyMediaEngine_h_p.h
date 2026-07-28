

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Mon Jan 18 19:14:07 2038
 */
/* Compiler settings for C:\Users\james\AppData\Local\Temp\PlayReadyMediaEngine.idl-6ef2b2f0:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.01.0628 
    protocol : all , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __PlayReadyMediaEngine_h_p_h__
#define __PlayReadyMediaEngine_h_p_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

#ifndef DECLSPEC_XFGVIRT
#if defined(_CONTROL_FLOW_GUARD_XFG)
#define DECLSPEC_XFGVIRT(base, func) __declspec(xfg_virtual(base, func))
#else
#define DECLSPEC_XFGVIRT(base, func)
#endif
#endif

#if defined(__cplusplus)
#if defined(__MIDL_USE_C_ENUM)
#define MIDL_ENUM enum
#else
#define MIDL_ENUM enum class
#endif
#endif


/* Forward Declarations */ 

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_FWD_DEFINED__ */


#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_FWD_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_FWD_DEFINED__
typedef interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs;

#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_FWD_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__
typedef interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine;

#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_FWD_DEFINED__ */


/* header files for imported files */
#include "Windows.Foundation.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0000 */
/* [local] */ 






































extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0000_v0_0_s_ifspec;

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("f047b6a3-e027-5b7f-b24b-a6572a45d0a8")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_LicenseServerUrl( 
            /* [retval][out] */ HSTRING *value) = 0;
        
        virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_LicenseServerUrl( 
            /* [in] */ HSTRING value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_CustomHeaders( 
            /* [retval][out] */ __FIMap_2_HSTRING_HSTRING **value) = 0;
        
        virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_CustomHeaders( 
            /* [in] */ __FIMap_2_HSTRING_HSTRING *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration, get_LicenseServerUrl)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_LicenseServerUrl )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [retval][out] */ HSTRING *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration, put_LicenseServerUrl)
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_LicenseServerUrl )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [in] */ HSTRING value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration, get_CustomHeaders)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_CustomHeaders )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [retval][out] */ __FIMap_2_HSTRING_HSTRING **value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration, put_CustomHeaders)
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_CustomHeaders )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration * This,
            /* [in] */ __FIMap_2_HSTRING_HSTRING *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfigurationVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_get_LicenseServerUrl(This,value)	\
    ( (This)->lpVtbl -> get_LicenseServerUrl(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_put_LicenseServerUrl(This,value)	\
    ( (This)->lpVtbl -> put_LicenseServerUrl(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_get_CustomHeaders(This,value)	\
    ( (This)->lpVtbl -> get_CustomHeaders(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_put_CustomHeaders(This,value)	\
    ( (This)->lpVtbl -> put_CustomHeaders(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("842ed4bc-5192-516b-ba39-d6ea690a5db5")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_OldState( 
            /* [retval][out] */ int *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_NewState( 
            /* [retval][out] */ int *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs, get_OldState)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_OldState )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [retval][out] */ int *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs, get_NewState)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_NewState )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs * This,
            /* [retval][out] */ int *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_get_OldState(This,value)	\
    ( (This)->lpVtbl -> get_OldState(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_get_NewState(This,value)	\
    ( (This)->lpVtbl -> get_NewState(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("ae97c6fb-745f-52f5-bad4-d9a2a31010e5")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreateInstance( 
            /* [in] */ int oldState,
            /* [in] */ int newState,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs **value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory, CreateInstance)
        HRESULT ( STDMETHODCALLTYPE *CreateInstance )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory * This,
            /* [in] */ int oldState,
            /* [in] */ int newState,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs **value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_CreateInstance(This,oldState,newState,value)	\
    ( (This)->lpVtbl -> CreateInstance(This,oldState,newState,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgsFactory_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("acbfc471-7914-5c50-b646-654b58658009")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_PositionSeconds( 
            /* [retval][out] */ double *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_DurationSeconds( 
            /* [retval][out] */ double *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs, get_PositionSeconds)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_PositionSeconds )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [retval][out] */ double *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs, get_DurationSeconds)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_DurationSeconds )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs * This,
            /* [retval][out] */ double *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_get_PositionSeconds(This,value)	\
    ( (This)->lpVtbl -> get_PositionSeconds(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_get_DurationSeconds(This,value)	\
    ( (This)->lpVtbl -> get_DurationSeconds(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("dbacd624-9fd9-52e2-9692-97323eb5af9b")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreateInstance( 
            /* [in] */ double positionSeconds,
            /* [in] */ double durationSeconds,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs **value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory, CreateInstance)
        HRESULT ( STDMETHODCALLTYPE *CreateInstance )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory * This,
            /* [in] */ double positionSeconds,
            /* [in] */ double durationSeconds,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs **value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_CreateInstance(This,positionSeconds,durationSeconds,value)	\
    ( (This)->lpVtbl -> CreateInstance(This,positionSeconds,durationSeconds,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgsFactory_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("6ea00545-d700-5174-b6b2-e64a6025dd86")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_ErrorCode( 
            /* [retval][out] */ int *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMessage( 
            /* [retval][out] */ HSTRING *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs, get_ErrorCode)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorCode )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [retval][out] */ int *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs, get_ErrorMessage)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMessage )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs * This,
            /* [retval][out] */ HSTRING *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_get_ErrorCode(This,value)	\
    ( (This)->lpVtbl -> get_ErrorCode(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_get_ErrorMessage(This,value)	\
    ( (This)->lpVtbl -> get_ErrorMessage(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("e7fcce98-c8c1-5808-97c1-7e150a094b12")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreateInstance( 
            /* [in] */ int errorCode,
            /* [in] */ HSTRING errorMessage,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs **value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory, CreateInstance)
        HRESULT ( STDMETHODCALLTYPE *CreateInstance )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory * This,
            /* [in] */ int errorCode,
            /* [in] */ HSTRING errorMessage,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs **value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_CreateInstance(This,errorCode,errorMessage,value)	\
    ( (This)->lpVtbl -> CreateInstance(This,errorCode,errorMessage,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgsFactory_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4d50bab2-f7a8-5e6d-8e5b-9a44e71b5beb")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_Width( 
            /* [retval][out] */ unsigned int *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_Height( 
            /* [retval][out] */ unsigned int *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs, get_Width)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Width )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [retval][out] */ unsigned int *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs, get_Height)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Height )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs * This,
            /* [retval][out] */ unsigned int *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_get_Width(This,value)	\
    ( (This)->lpVtbl -> get_Width(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_get_Height(This,value)	\
    ( (This)->lpVtbl -> get_Height(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("6d3cb95c-6ccb-513f-a4f3-1b1cbeb20a30")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreateInstance( 
            /* [in] */ unsigned int width,
            /* [in] */ unsigned int height,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs **value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory, CreateInstance)
        HRESULT ( STDMETHODCALLTYPE *CreateInstance )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory * This,
            /* [in] */ unsigned int width,
            /* [in] */ unsigned int height,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs **value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_CreateInstance(This,width,height,value)	\
    ( (This)->lpVtbl -> CreateInstance(This,width,height,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgsFactory_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("844724e6-73b2-5bdd-8ce9-9572b002b855")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_Success( 
            /* [retval][out] */ boolean *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMessage( 
            /* [retval][out] */ HSTRING *value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs, get_Success)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Success )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [retval][out] */ boolean *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs, get_ErrorMessage)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMessage )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs * This,
            /* [retval][out] */ HSTRING *value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_get_Success(This,value)	\
    ( (This)->lpVtbl -> get_Success(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_get_ErrorMessage(This,value)	\
    ( (This)->lpVtbl -> get_ErrorMessage(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__ */


#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("9aff49d5-4b2e-5ffb-baf6-b9dd7c88ce4e")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreateInstance( 
            /* [in] */ boolean success,
            /* [in] */ HSTRING errorMessage,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs **value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory, CreateInstance)
        HRESULT ( STDMETHODCALLTYPE *CreateInstance )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory * This,
            /* [in] */ boolean success,
            /* [in] */ HSTRING errorMessage,
            /* [out][retval] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs **value);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_CreateInstance(This,success,errorMessage,value)	\
    ( (This)->lpVtbl -> CreateInstance(This,success,errorMessage,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgsFactory_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0222 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0222_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0222_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0012 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0012_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0012_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("be4e81ec-3897-5d2a-bcfb-7fec8951002e")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ IInspectable *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectableVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ IInspectable *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0013 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0013_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0013_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0223 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0223_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0223_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0014 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0014_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0014_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("3782bac1-baca-577f-8a3a-783258fa6451")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineErrorEventArgs *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0015 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0015_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0015_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0224 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0224_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0224_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0016 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0016_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0016_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("f9c949b2-9613-5632-bb5f-3870ed8c6f4f")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineTimeChangedEventArgs *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0017 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0017_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0017_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0225 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0225_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0225_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0018 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0018_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0018_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1100e79e-036a-519b-ba68-ee3a8dcdd230")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ boolean e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_booleanVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ boolean e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0019 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0019_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0019_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0226 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0226_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0226_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0020 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0020_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0020_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("87e20b1f-82c0-52b9-b910-be69930c5b27")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineSizeChangedEventArgs *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0021 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0021_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0021_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0227 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0227_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0227_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0022 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0022_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0022_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("f37c9a1e-2692-5893-b3f4-25331ab7dcff")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIDrmLicenseAcquiredEventArgs *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0023 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0023_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0023_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0228 */




extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0228_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine2Eidl_0000_0228_v0_0_s_ifspec;

/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0024 */
/* [local] */ 

#ifndef DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs
#define DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0024_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0024_v0_0_s_ifspec;

#ifndef ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__
#define ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__

/* interface __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("66ac6990-648c-5a0f-a1d6-13e2679e8906")
    __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs *e) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgsVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This);
        
        DECLSPEC_XFGVIRT(__FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs, Invoke)
        HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs * This,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine *sender,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIMediaEngineStateChangedEventArgs *e);
        
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


#endif 	/* C style interface */




#endif 	/* ____FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_PlayReadyMediaEngine_0000_0025 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs */


extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0025_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_PlayReadyMediaEngine_0000_0025_v0_0_s_ifspec;

#ifndef ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__
#define ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__

/* interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine */
/* [object][uuid] */ 


EXTERN_C const IID IID___x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("3725974e-d708-5110-8b38-9824de4352a3")
    __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Initialize( 
            /* [in] */ __int64 swapChainPanelHandle) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetSource( 
            /* [in] */ HSTRING url,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration *drmConfig) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetSourceFromManifestBytes( 
            /* [in] */ __FIVectorView_1_byte *manifestData,
            /* [in] */ HSTRING originalUrl,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration *drmConfig) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Play( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Pause( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Seek( 
            /* [in] */ double positionSeconds) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetVolume( 
            /* [in] */ double volume) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetMuted( 
            /* [in] */ boolean muted) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetPlaybackRate( 
            /* [in] */ double rate) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetLooping( 
            /* [in] */ boolean looping) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetStretch( 
            /* [in] */ int stretchMode) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ResizeSwapChain( 
            /* [in] */ unsigned int width,
            /* [in] */ unsigned int height) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Close( void) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_CurrentPositionSeconds( 
            /* [retval][out] */ double *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_DurationSeconds( 
            /* [retval][out] */ double *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_NaturalVideoWidth( 
            /* [retval][out] */ unsigned int *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_NaturalVideoHeight( 
            /* [retval][out] */ unsigned int *value) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_IsBuffering( 
            /* [retval][out] */ boolean *value) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_MediaOpened( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_MediaOpened( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_MediaFailed( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_MediaFailed( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_MediaEnded( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_MediaEnded( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_TimeChanged( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_TimeChanged( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_BufferingChanged( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_BufferingChanged( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_SizeChanged( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_SizeChanged( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_LicenseAcquired( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_LicenseAcquired( 
            /* [in] */ EventRegistrationToken token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE add_StateChanged( 
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE remove_StateChanged( 
            /* [in] */ EventRegistrationToken token) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This);
        
        DECLSPEC_XFGVIRT(IInspectable, GetIids)
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        DECLSPEC_XFGVIRT(IInspectable, GetRuntimeClassName)
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [out] */ HSTRING *className);
        
        DECLSPEC_XFGVIRT(IInspectable, GetTrustLevel)
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [out] */ TrustLevel *trustLevel);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, Initialize)
        HRESULT ( STDMETHODCALLTYPE *Initialize )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __int64 swapChainPanelHandle);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetSource)
        HRESULT ( STDMETHODCALLTYPE *SetSource )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ HSTRING url,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration *drmConfig);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetSourceFromManifestBytes)
        HRESULT ( STDMETHODCALLTYPE *SetSourceFromManifestBytes )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FIVectorView_1_byte *manifestData,
            /* [in] */ HSTRING originalUrl,
            /* [in] */ __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyDrmConfiguration *drmConfig);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, Play)
        HRESULT ( STDMETHODCALLTYPE *Play )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, Pause)
        HRESULT ( STDMETHODCALLTYPE *Pause )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, Seek)
        HRESULT ( STDMETHODCALLTYPE *Seek )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ double positionSeconds);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetVolume)
        HRESULT ( STDMETHODCALLTYPE *SetVolume )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ double volume);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetMuted)
        HRESULT ( STDMETHODCALLTYPE *SetMuted )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ boolean muted);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetPlaybackRate)
        HRESULT ( STDMETHODCALLTYPE *SetPlaybackRate )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ double rate);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetLooping)
        HRESULT ( STDMETHODCALLTYPE *SetLooping )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ boolean looping);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, SetStretch)
        HRESULT ( STDMETHODCALLTYPE *SetStretch )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ int stretchMode);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, ResizeSwapChain)
        HRESULT ( STDMETHODCALLTYPE *ResizeSwapChain )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ unsigned int width,
            /* [in] */ unsigned int height);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, Close)
        HRESULT ( STDMETHODCALLTYPE *Close )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, get_CurrentPositionSeconds)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_CurrentPositionSeconds )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [retval][out] */ double *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, get_DurationSeconds)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_DurationSeconds )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [retval][out] */ double *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, get_NaturalVideoWidth)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_NaturalVideoWidth )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [retval][out] */ unsigned int *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, get_NaturalVideoHeight)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_NaturalVideoHeight )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [retval][out] */ unsigned int *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, get_IsBuffering)
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsBuffering )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [retval][out] */ boolean *value);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_MediaOpened)
        HRESULT ( STDMETHODCALLTYPE *add_MediaOpened )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_MediaOpened)
        HRESULT ( STDMETHODCALLTYPE *remove_MediaOpened )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_MediaFailed)
        HRESULT ( STDMETHODCALLTYPE *add_MediaFailed )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineErrorEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_MediaFailed)
        HRESULT ( STDMETHODCALLTYPE *remove_MediaFailed )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_MediaEnded)
        HRESULT ( STDMETHODCALLTYPE *add_MediaEnded )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_IInspectable *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_MediaEnded)
        HRESULT ( STDMETHODCALLTYPE *remove_MediaEnded )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_TimeChanged)
        HRESULT ( STDMETHODCALLTYPE *add_TimeChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineTimeChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_TimeChanged)
        HRESULT ( STDMETHODCALLTYPE *remove_TimeChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_BufferingChanged)
        HRESULT ( STDMETHODCALLTYPE *add_BufferingChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_boolean *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_BufferingChanged)
        HRESULT ( STDMETHODCALLTYPE *remove_BufferingChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_SizeChanged)
        HRESULT ( STDMETHODCALLTYPE *add_SizeChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineSizeChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_SizeChanged)
        HRESULT ( STDMETHODCALLTYPE *remove_SizeChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_LicenseAcquired)
        HRESULT ( STDMETHODCALLTYPE *add_LicenseAcquired )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CDrmLicenseAcquiredEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_LicenseAcquired)
        HRESULT ( STDMETHODCALLTYPE *remove_LicenseAcquired )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, add_StateChanged)
        HRESULT ( STDMETHODCALLTYPE *add_StateChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ __FITypedEventHandler_2_CommunityToolkit__CMaui__CMedia__CWinRT__CPlayReadyMediaEngine_CommunityToolkit__CMaui__CMedia__CWinRT__CMediaEngineStateChangedEventArgs *handler,
            /* [out][retval] */ EventRegistrationToken *token);
        
        DECLSPEC_XFGVIRT(__x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine, remove_StateChanged)
        HRESULT ( STDMETHODCALLTYPE *remove_StateChanged )( 
            __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine * This,
            /* [in] */ EventRegistrationToken token);
        
        END_INTERFACE
    } __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl;

    interface __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine
    {
        CONST_VTBL struct __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngineVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Initialize(This,swapChainPanelHandle)	\
    ( (This)->lpVtbl -> Initialize(This,swapChainPanelHandle) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetSource(This,url,drmConfig)	\
    ( (This)->lpVtbl -> SetSource(This,url,drmConfig) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetSourceFromManifestBytes(This,manifestData,originalUrl,drmConfig)	\
    ( (This)->lpVtbl -> SetSourceFromManifestBytes(This,manifestData,originalUrl,drmConfig) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Play(This)	\
    ( (This)->lpVtbl -> Play(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Pause(This)	\
    ( (This)->lpVtbl -> Pause(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Seek(This,positionSeconds)	\
    ( (This)->lpVtbl -> Seek(This,positionSeconds) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetVolume(This,volume)	\
    ( (This)->lpVtbl -> SetVolume(This,volume) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetMuted(This,muted)	\
    ( (This)->lpVtbl -> SetMuted(This,muted) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetPlaybackRate(This,rate)	\
    ( (This)->lpVtbl -> SetPlaybackRate(This,rate) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetLooping(This,looping)	\
    ( (This)->lpVtbl -> SetLooping(This,looping) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_SetStretch(This,stretchMode)	\
    ( (This)->lpVtbl -> SetStretch(This,stretchMode) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_ResizeSwapChain(This,width,height)	\
    ( (This)->lpVtbl -> ResizeSwapChain(This,width,height) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_Close(This)	\
    ( (This)->lpVtbl -> Close(This) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_CurrentPositionSeconds(This,value)	\
    ( (This)->lpVtbl -> get_CurrentPositionSeconds(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_DurationSeconds(This,value)	\
    ( (This)->lpVtbl -> get_DurationSeconds(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_NaturalVideoWidth(This,value)	\
    ( (This)->lpVtbl -> get_NaturalVideoWidth(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_NaturalVideoHeight(This,value)	\
    ( (This)->lpVtbl -> get_NaturalVideoHeight(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_get_IsBuffering(This,value)	\
    ( (This)->lpVtbl -> get_IsBuffering(This,value) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaOpened(This,handler,token)	\
    ( (This)->lpVtbl -> add_MediaOpened(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaOpened(This,token)	\
    ( (This)->lpVtbl -> remove_MediaOpened(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaFailed(This,handler,token)	\
    ( (This)->lpVtbl -> add_MediaFailed(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaFailed(This,token)	\
    ( (This)->lpVtbl -> remove_MediaFailed(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_MediaEnded(This,handler,token)	\
    ( (This)->lpVtbl -> add_MediaEnded(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_MediaEnded(This,token)	\
    ( (This)->lpVtbl -> remove_MediaEnded(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_TimeChanged(This,handler,token)	\
    ( (This)->lpVtbl -> add_TimeChanged(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_TimeChanged(This,token)	\
    ( (This)->lpVtbl -> remove_TimeChanged(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_BufferingChanged(This,handler,token)	\
    ( (This)->lpVtbl -> add_BufferingChanged(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_BufferingChanged(This,token)	\
    ( (This)->lpVtbl -> remove_BufferingChanged(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_SizeChanged(This,handler,token)	\
    ( (This)->lpVtbl -> add_SizeChanged(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_SizeChanged(This,token)	\
    ( (This)->lpVtbl -> remove_SizeChanged(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_LicenseAcquired(This,handler,token)	\
    ( (This)->lpVtbl -> add_LicenseAcquired(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_LicenseAcquired(This,token)	\
    ( (This)->lpVtbl -> remove_LicenseAcquired(This,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_add_StateChanged(This,handler,token)	\
    ( (This)->lpVtbl -> add_StateChanged(This,handler,token) ) 

#define __x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_remove_StateChanged(This,token)	\
    ( (This)->lpVtbl -> remove_StateChanged(This,token) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_ABI_CCommunityToolkit_CMaui_CMedia_CWinRT_CIPlayReadyMediaEngine_INTERFACE_DEFINED__ */


/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  HSTRING_UserSize(     unsigned long *, unsigned long            , HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserMarshal(  unsigned long *, unsigned char *, HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserUnmarshal(unsigned long *, unsigned char *, HSTRING * ); 
void                      __RPC_USER  HSTRING_UserFree(     unsigned long *, HSTRING * ); 

unsigned long             __RPC_USER  HSTRING_UserSize64(     unsigned long *, unsigned long            , HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserMarshal64(  unsigned long *, unsigned char *, HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserUnmarshal64(unsigned long *, unsigned char *, HSTRING * ); 
void                      __RPC_USER  HSTRING_UserFree64(     unsigned long *, HSTRING * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


