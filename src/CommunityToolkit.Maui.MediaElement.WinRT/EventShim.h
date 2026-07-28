#pragma once

#include "pch.h"

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	/// <summary>
	/// Backward-compatible wrapper around winrt::event&lt;Delegate&gt; for C++/WinRT 2.0+ / 3.0+.
	///
	/// Problem: In C++/WinRT 2.0+, winrt::event&lt;Delegate&gt; uses add()/remove() for
	/// subscription management and operator()(Args...) for firing. However, the
	/// cppwinrt.exe-generated produce&lt;&gt; code still emits calls to
	/// shim().EventName(handler) and shim().EventName(token), which resolve to
	/// operator() — causing a compile-time error because operator() now returns
	/// void (fire path) instead of event_token (old add path).
	///
	/// This shim adds operator()(delegate) and operator()(event_token) overloads
	/// that delegate to add() and remove() respectively, while preserving the
	/// fire operator() via a using-declaration.
	/// </summary>
	template <typename Delegate>
	struct event_shim : winrt::event<Delegate>
	{
		using base_type = winrt::event<Delegate>;
		using delegate_type = Delegate;

		// Bring in the fire operator()(Args...) from the base
		using base_type::operator();

		// Old-style add: returns event_token
		winrt::event_token operator()(delegate_type const& handler)
		{
			return this->add(handler);
		}

		// Old-style remove: called with event_token
		void operator()(winrt::event_token const& token)
		{
			this->remove(token);
		}
	};
}
