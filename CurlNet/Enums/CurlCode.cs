namespace CurlNet.Enums
{
	public enum CurlCode
	{
		/// <summary>
		/// Internal, do not use.
		/// </summary>
		NotInitialized = -1,
		/// <summary>
		/// All fine. Proceed as usual.
		/// </summary>
		Ok = 0,
		/// <summary>
		/// The URL you passed to libcurl used a protocol that this libcurl does not support. The support might be a compile-time option that you didn't use, it can be a misspelled protocol string or just a protocol libcurl has no code for.
		/// </summary>
		UnsupportedProtocol = 1,
		/// <summary>
		/// Very early initialization code failed. This is likely to be an internal error or problem, or a resource problem where something fundamental couldn't get done at init time.
		/// </summary>
		FailedInit = 2,
		/// <summary>
		/// The URL was not properly formatted.
		/// </summary>
		UrlMalformat = 3,
		/// <summary>
		/// A requested feature, protocol or option was not found built-in in this libcurl due to a build-time decision. This means that a feature or option was not enabled or explicitly disabled when libcurl was built and in order to get it to function you have to get a rebuilt libcurl.
		/// </summary>
		NotBuiltIn = 4,
		/// <summary>
		/// Couldn't resolve proxy. The given proxy host could not be resolved.
		/// </summary>
		CouldntResolveProxy = 5,
		/// <summary>
		/// Couldn't resolve host. The given remote host was not resolved.
		/// </summary>
		CouldntResolveHost = 6,
		/// <summary>
		/// Failed to connect() to host or proxy.
		/// </summary>
		CouldntConnect = 7,
		/// <summary>
		/// The server sent data libcurl couldn't parse.
		/// </summary>
		WeirdServerReply = 8,
		/// <summary>
		/// We were denied access to the resource given in the URL. For FTP, this occurs while trying to change to the remote directory.
		/// </summary>
		RemoteAccessDenied = 9,
		/// <summary>
		/// While waiting for the server to connect back when an active FTP session is used, an error code was sent over the control connection or similar.
		/// </summary>
		FtpAcceptFailed = 10,
		/// <summary>
		/// After having sent the FTP password to the server, libcurl expects a proper reply. This error code indicates that an unexpected code was returned.
		/// </summary>
		FtpWeirdPassReply = 11,
		/// <summary>
		/// During an active FTP session while waiting for the server to connect, the CURLOPT_ACCEPTTIMEOUT_MS (or the internal default) timeout expired.
		/// </summary>
		FtpAcceptTimeout = 12,
		/// <summary>
		/// libcurl failed to get a sensible result back from the server as a response to either a PASV or a EPSV command. The server is flawed.
		/// </summary>
		FtpWeirdPasvReply = 13,
		/// <summary>
		/// FTP servers return a 227-line as a response to a PASV command. If libcurl fails to parse that line, this return code is passed back.
		/// </summary>
		FtpWeird227Format = 14,
		/// <summary>
		/// An internal failure to lookup the host used for the new connection.
		/// </summary>
		FtpCantGetHost = 15,
		/// <summary>
		/// A problem was detected in the HTTP2 framing layer. This is somewhat generic and can be one out of several problems, see the error buffer for details.
		/// </summary>
		Http2 = 16,
		/// <summary>
		/// Received an error when trying to set the transfer mode to binary or ASCII.
		/// </summary>
		FtpCouldntSetType = 17,
		/// <summary>
		/// A file transfer was shorter or larger than expected. This happens when the server first reports an expected transfer size, and then delivers data that doesn't match the previously given size.
		/// </summary>
		PartialFile = 18,
		/// <summary>
		/// This was either a weird reply to a 'RETR' command or a zero byte transfer complete.
		/// </summary>
		FtpCouldntRetrFile = 19,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete20 = 20,
		/// <summary>
		/// When sending custom "QUOTE" commands to the remote server, one of the commands returned an error code that was 400 or higher (for FTP) or otherwise indicated unsuccessful completion of the command.
		/// </summary>
		QuoteError = 21,
		/// <summary>
		/// This is returned if CURLOPT_FAILONERROR is set TRUE and the HTTP server returns an error code that is >= 400.
		/// </summary>
		HttpReturnedError = 22,
		/// <summary>
		/// An error occurred when writing received data to a local file, or an error was returned to libcurl from a write callback.
		/// </summary>
		WriteError = 23,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete24 = 24,
		/// <summary>
		/// Failed starting the upload. For FTP, the server typically denied the STOR command. The error buffer usually contains the server's explanation for this.
		/// </summary>
		UploadFailed = 25,
		/// <summary>
		/// There was a problem reading a local file or an error returned by the read callback.
		/// </summary>
		ReadError = 26,
		/// <summary>
		/// A memory allocation request failed. This is serious badness and things are severely screwed up if this ever occurs.
		/// </summary>
		OutOfMemory = 27,
		/// <summary>
		/// Operation timeout. The specified time-out period was reached according to the conditions.
		/// </summary>
		OperationTimedout = 28,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete29 = 29,
		/// <summary>
		/// The FTP PORT command returned error. This mostly happens when you haven't specified a good enough address for libcurl to use. See CURLOPT_FTPPORT.
		/// </summary>
		FtpPortFailed = 30,
		/// <summary>
		/// The FTP REST command returned error. This should never happen if the server is sane.
		/// </summary>
		FtpCouldntUseRest = 31,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete32 = 32,
		/// <summary>
		/// The server does not support or accept range requests.
		/// </summary>
		RangeError = 33,
		/// <summary>
		/// This is an odd error that mainly occurs due to internal confusion.
		/// </summary>
		HttpPostError = 34,
		/// <summary>
		/// A problem occurred somewhere in the SSL/TLS handshake. You really want the error buffer and read the message there as it pinpoints the problem slightly more. Could be certificates (file formats, paths, permissions), passwords, and others.
		/// </summary>
		SslConnectError = 35,
		/// <summary>
		/// The download could not be resumed because the specified offset was out of the file boundary.
		/// </summary>
		BadDownloadResume = 36,
		/// <summary>
		/// A file given with FILE:// couldn't be opened. Most likely because the file path doesn't identify an existing file. Did you check file permissions?
		/// </summary>
		FileCouldntReadFile = 37,
		/// <summary>
		/// LDAP cannot bind. LDAP bind operation failed.
		/// </summary>
		LdapCannotBind = 38,
		/// <summary>
		/// LDAP search failed.
		/// </summary>
		LdapSearchFailed = 39,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete40 = 40,
		/// <summary>
		/// Function not found. A required zlib function was not found.
		/// </summary>
		FunctionNotFound = 41,
		/// <summary>
		/// Aborted by callback. A callback returned "abort" to libcurl.
		/// </summary>
		AbortedByCallback = 42,
		/// <summary>
		/// Internal error. A function was called with a bad parameter.
		/// </summary>
		BadFunctionArgument = 43,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete44 = 44,
		/// <summary>
		/// Interface error. A specified outgoing interface could not be used. Set which interface to use for outgoing connections' source IP address with CURLOPT_INTERFACE.
		/// </summary>
		InterfaceFailed = 45,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete46 = 46,
		/// <summary>
		/// Too many redirects. When following redirects, libcurl hit the maximum amount. Set your limit with CURLOPT_MAXREDIRS.
		/// </summary>
		TooManyRedirects = 47,
		/// <summary>
		/// An option passed to libcurl is not recognized/known. Refer to the appropriate documentation. This is most likely a problem in the program that uses libcurl. The error buffer might contain more specific information about which exact option it concerns.
		/// </summary>
		UnknownOption = 48,
		/// <summary>
		/// A telnet option string was Illegally formatted.
		/// </summary>
		TelnetOptionSyntax = 49,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete50 = 50,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete51 = 51,
		/// <summary>
		/// Nothing was returned from the server, and under the circumstances, getting nothing is considered an error.
		/// </summary>
		GotNothing = 52,
		/// <summary>
		/// The specified crypto engine wasn't found.
		/// </summary>
		SslEngineNotfound = 53,
		/// <summary>
		/// Failed setting the selected SSL crypto engine as default!
		/// </summary>
		SslEngineSetfailed = 54,
		/// <summary>
		/// Failed sending network data.
		/// </summary>
		SendError = 55,
		/// <summary>
		/// Failure with receiving network data.
		/// </summary>
		RecvError = 56,
		/// <summary>
		/// Not used.
		/// </summary>
		Obsolete57 = 57,
		/// <summary>
		/// Problem with the local client certificate.
		/// </summary>
		SslCertproblem = 58,
		/// <summary>
		/// Couldn't use specified cipher.
		/// </summary>
		SslCipher = 59,
		/// <summary>
		/// The remote server's SSL certificate or SSH md5 fingerprint was deemed not OK. This error code has been unified with CURLE_SSL_CACERT since 7.62.0. Its previous value was 51.
		/// </summary>
		PeerFailedVerification = 60,
		/// <summary>
		/// Unrecognized transfer encoding.
		/// </summary>
		BadContentEncoding = 61,
		/// <summary>
		/// Invalid LDAP URL.
		/// </summary>
		LdapInvalidUrl = 62,
		/// <summary>
		/// Maximum file size exceeded.
		/// </summary>
		FilesizeExceeded = 63,
		/// <summary>
		/// Requested FTP SSL level failed.
		/// </summary>
		UseSslFailed = 64,
		/// <summary>
		/// When doing a send operation curl had to rewind the data to retransmit, but the rewinding operation failed.
		/// </summary>
		SendFailRewind = 65,
		/// <summary>
		/// Initiating the SSL Engine failed.
		/// </summary>
		SslEngineInitfailed = 66,
		/// <summary>
		/// The remote server denied curl to login.
		/// </summary>
		LoginDenied = 67,
		/// <summary>
		/// File not found on TFTP server.
		/// </summary>
		TftpNotfound = 68,
		/// <summary>
		/// Permission problem on TFTP server.
		/// </summary>
		TftpPerm = 69,
		/// <summary>
		/// Out of disk space on the server.
		/// </summary>
		RemoteDiskFull = 70,
		/// <summary>
		/// Illegal TFTP operation.
		/// </summary>
		TftpIllegal = 71,
		/// <summary>
		/// Unknown TFTP transfer ID.
		/// </summary>
		TftpUnknownid = 72,
		/// <summary>
		/// File already exists and will not be overwritten.
		/// </summary>
		RemoteFileExists = 73,
		/// <summary>
		/// This error should never be returned by a properly functioning TFTP server.
		/// </summary>
		TftpNosuchuser = 74,
		/// <summary>
		/// Character conversion failed.
		/// </summary>
		ConvFailed = 75,
		/// <summary>
		/// Caller must register conversion callbacks.
		/// </summary>
		ConvReqd = 76,
		/// <summary>
		/// Problem with reading the SSL CA cert (path? access rights?).
		/// </summary>
		SslCacertBadfile = 77,
		/// <summary>
		/// The resource referenced in the URL does not exist.
		/// </summary>
		RemoteFileNotFound = 78,
		/// <summary>
		/// An unspecified error occurred during the SSH session.
		/// </summary>
		Ssh = 79,
		/// <summary>
		/// Failed to shut down the SSL connection.
		/// </summary>
		SslShutdownFailed = 80,
		/// <summary>
		/// Socket is not ready for send/recv wait till it's ready and try again. This return code is only returned from curl_easy_recv and curl_easy_send
		/// </summary>
		Again = 81,
		/// <summary>
		/// Failed to load CRL file.
		/// </summary>
		SslCrlBadfile = 82,
		/// <summary>
		/// Issuer check failed.
		/// </summary>
		SslIssuerError = 83,
		/// <summary>
		/// The FTP server does not understand the PRET command at all or does not support the given argument. Be careful when using CURLOPT_CUSTOMREQUEST, a custom LIST command will be sent with PRET CMD before PASV as well.
		/// </summary>
		FtpPretFailed = 84,
		/// <summary>
		/// Mismatch of RTSP CSeq numbers.
		/// </summary>
		RtspCseqError = 85,
		/// <summary>
		/// Mismatch of RTSP Session Identifiers.
		/// </summary>
		RtspSessionError = 86,
		/// <summary>
		/// Unable to parse FTP file list (during FTP wildcard downloading).
		/// </summary>
		FtpBadFileList = 87,
		/// <summary>
		/// Chunk callback reported error.
		/// </summary>
		ChunkFailed = 88,
		/// <summary>
		/// (For internal use only, will never be returned by libcurl) No connection available, the session will be queued.
		/// </summary>
		NoConnectionAvailable = 89,
		/// <summary>
		/// Failed to match the pinned key specified with CURLOPT_PINNEDPUBLICKEY.
		/// </summary>
		SslPinnedPubKeyNotMatch = 90,
		/// <summary>
		/// Status returned failure when asked with CURLOPT_SSL_VERIFYSTATUS.
		/// </summary>
		SslInvalidCertStatus = 91,
		/// <summary>
		/// Stream error in the HTTP/2 framing layer.
		/// </summary>
		Http2Stream = 92,
		/// <summary>
		/// An API function was called from inside a callback.
		/// </summary>
		RecursiveApiCall = 93,
		/// <summary>
		/// An authentication function returned an error.
		/// </summary>
		AuthError = 94,
		/// <summary>
		/// A problem was detected in the HTTP/3 layer. This is somewhat generic and can be one out of several problems, see the error buffer for details.
		/// </summary>
		Http3 = 95
	}
}
