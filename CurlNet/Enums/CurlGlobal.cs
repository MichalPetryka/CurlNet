using System;

namespace CurlNet.Enums
{
	/// <summary>
	/// The flags option is a bit pattern that tells libcurl exactly what features to init, as described below. Set the desired bits by ORing the values together. In normal operation, you must specify CURL_GLOBAL_ALL. Don't use any other value unless you are familiar with it and mean to control internal operations of libcurl.
	/// </summary>
	[Flags]
	internal enum CurlGlobal
	{
		/// <summary>
		/// Initialise nothing extra. This sets no bit.
		/// </summary>
		Nothing = 0,
		/// <summary>
		/// Initialize SSL.
		/// The implication here is that if this bit is not set, the initialization of the SSL layer needs to be done by the application or at least outside of libcurl. The exact procedure how to do SSL initialization depends on the TLS backend libcurl uses.
		/// Doing TLS based transfers without having the TLS layer initialized may lead to unexpected behaviors.
		/// </summary>
		Ssl = 1,
		/// <summary>
		/// Initialize the Win32 socket libraries.
		/// The implication here is that if this bit is not set, the initialization of winsock has to be done by the application or you risk getting undefined behaviors. This option exists for when the initialization is handled outside of libcurl so there's no need for libcurl to do it again.
		/// </summary>
		Win32 = 1 << 1,
		/// <summary>
		/// When this flag is set, curl will acknowledge EINTR condition when connecting or when waiting for data. Otherwise, curl waits until full timeout elapses.
		/// </summary>
		AckEintr = 1 << 2,
		/// <summary>
		/// Initialize everything possible. This sets all known bits except AckEintr.
		/// </summary>
		All = Ssl | Win32,
		/// <summary>
		/// A sensible default. It will init both SSL and Win32. Right now, this equals the functionality of the All mask.
		/// </summary>
		Default = All
	}
}
