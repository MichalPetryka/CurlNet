namespace CurlNet.Enums
{
	internal enum CurlOption : uint
	{
		/// <summary>
		/// Data pointer to pass to the write callback.
		/// </summary>
		WriteData = CurlOperationType.ObjectPointer + 1,
		/// <summary>
		/// URL to work on.
		/// </summary>
		Url = CurlOperationType.StringPointer + 2,
		/// <summary>
		/// Port number to connect to.
		/// </summary>
		Port = CurlOperationType.Long + 3,
		/// <summary>
		/// Proxy to use.
		/// </summary>
		Proxy = CurlOperationType.StringPointer + 4,

		/* "user:password;options" to use when fetching. */
		Userpwd = CurlOperationType.StringPointer + 5,

		/* "user:password" to use with proxy. */
		Proxyuserpwd = CurlOperationType.StringPointer + 6,

		/* Range to get, specified as an ASCII string. */
		Range = CurlOperationType.StringPointer + 7,
		/// <summary>
		/// Data pointer to pass to the read callback.
		/// </summary>
		ReadData = CurlOperationType.ObjectPointer + 9,
		/// <summary>
		/// Error message buffer.
		/// </summary>
		Errorbuffer = CurlOperationType.ObjectPointer + 10,
		/// <summary>
		/// Callback for writing data.
		/// </summary>
		WriteFunction = CurlOperationType.FunctionPointer + 11,
		/// <summary>
		/// Callback for reading data.
		/// </summary>
		ReadFunction = CurlOperationType.FunctionPointer + 12,

		/* Time-out the read operation after this amount of seconds */
		Timeout = CurlOperationType.Long + 13,

		/* If the CURLOPT_INFILE is used, this can be used to inform libcurl about
		 * how large the file being sent really is. That allows better error
		 * checking and better verifies that the upload was successful. -1 means
		 * unknown size.
		 *
		 * For large file support, there is also a _LARGE version of the key
		 * which takes an off_t type, allowing platforms with larger off_t
		 * sizes to handle larger files.  See below for INFILESIZE_LARGE.
		 */
		Infilesize = CurlOperationType.Long + 14,

		/* POST static input fields. */
		Postfields = CurlOperationType.ObjectPointer + 15,

		/* Set the referrer page (needed by some CGIs) */
		Referer = CurlOperationType.StringPointer + 16,

		/* Set the FTP PORT string (interface name, named or numerical IP address)
		   Use i.e '-' to use default address. */
		Ftpport = CurlOperationType.StringPointer + 17,

		/* Set the User-Agent string (examined by some CGIs) */
		Useragent = CurlOperationType.StringPointer + 18,

		/* If the download receives less than "low speed limit" bytes/second
		 * during "low speed time" seconds, the operations is aborted.
		 * You could i.e if you have a pretty high speed connection, abort if
		 * it is less than 2000 bytes/sec during 20 seconds.
		 */

		/* Set the "low speed limit" */
		LowSpeedLimit = CurlOperationType.Long + 19,

		/* Set the "low speed time" */
		LowSpeedTime = CurlOperationType.Long + 20,

		/* Set the continuation offset.
		 *
		 * Note there is also a _LARGE version of this key which uses
		 * off_t types, allowing for large file offsets on platforms which
		 * use larger-than-32-bit off_t's.  Look below for RESUME_FROM_LARGE.
		 */
		ResumeFrom = CurlOperationType.Long + 21,

		/* Set cookie in request: */
		Cookie = CurlOperationType.StringPointer + 22,

		/* This points to a linked list of headers, struct curl_slist kind. This
		   list is also used for RTSP (in spite of its name) */
		Httpheader = CurlOperationType.ObjectPointer + 23,

		/* This points to a linked list of post entries, struct curl_httppost */
		Httppost = CurlOperationType.ObjectPointer + 24,

		/* name of the file keeping your private SSL-certificate */
		Sslcert = CurlOperationType.StringPointer + 25,

		/* password for the SSL or SSH private key */
		Keypasswd = CurlOperationType.StringPointer + 26,

		/* send TYPE parameter? */
		Crlf = CurlOperationType.Long + 27,

		/* send linked-list of QUOTE commands */
		Quote = CurlOperationType.ObjectPointer + 28,
		/// <summary>
		/// Data pointer to pass to the header callback.
		/// </summary>
		HeaderData = CurlOperationType.ObjectPointer + 29,

		/* point to a file to read the initial cookies from, also enables
		   "cookie awareness" */
		Cookiefile = CurlOperationType.StringPointer + 31,

		/* What version to specifically try to use.
		   See CURL_SSLVERSION defines below. */
		Sslversion = CurlOperationType.Long + 32,

		/* What kind of HTTP time condition to use, see defines */
		Timecondition = CurlOperationType.Long + 33,

		/* Time to use with the above condition. Specified in number of seconds
		   since 1 Jan 1970 */
		Timevalue = CurlOperationType.Long + 34,

		/* 35 = OBSOLETE */

		/* Custom request, for customizing the get command like
		   HTTP: DELETE, TRACE and others
		   FTP: to use a different list command
		   */
		Customrequest = CurlOperationType.StringPointer + 36,
		/// <summary>
		/// stderr replacement stream.
		/// </summary>
		Stderr = CurlOperationType.ObjectPointer + 37,

		/* 38 is not used */

		/* send linked-list of post-transfer QUOTE commands */
		Postquote = CurlOperationType.ObjectPointer + 39,

		Obsolete40 = CurlOperationType.ObjectPointer + 40, /* OBSOLETE, do not use! */
		/// <summary>
		/// Display verbose information.
		/// </summary>
		Verbose = CurlOperationType.Long + 41,
		/// <summary>
		/// Include the header in the body output.
		/// </summary>
		Header = CurlOperationType.Long + 42,
		/// <summary>
		/// Shut off the progress meter.
		/// </summary>
		NoProgress = CurlOperationType.Long + 43,   /* shut off the progress meter */
		Nobody = CurlOperationType.Long + 44,
		/// <summary>
		/// Fail on HTTP 4xx errors.
		/// </summary>
		Failonerror = CurlOperationType.Long + 45,  /* no output on http error codes >= 400 */
		Upload = CurlOperationType.Long + 46,       /* this is an upload */
		Post = CurlOperationType.Long + 47,         /* HTTP POST method */
		Dirlistonly = CurlOperationType.Long + 48,  /* bare names when listing directories */

		Append = CurlOperationType.Long + 50,       /* Append instead of overwrite on upload! */

		/* Specify whether to read the user+password from the .netrc or the URL.
		 * This must be one of the CURL_NETRC_* enums below. */
		Netrc = CurlOperationType.Long + 51,

		Followlocation = CurlOperationType.Long + 52,  /* use Location: Luke! */

		Transfertext = CurlOperationType.Long + 53, /* transfer data in text/ASCII format */
		Put = CurlOperationType.Long + 54,          /* HTTP PUT */
		/// <summary>
		/// OBSOLETE callback for progress meter.
		/// </summary>
		ProgressFunction = CurlOperationType.FunctionPointer + 56,
		/// <summary>
		/// Data pointer to pass to the progress meter callback.
		/// </summary>
		ProgressData = CurlOperationType.ObjectPointer + 57,

		/* We want the referrer field set automatically when following locations */
		Autoreferer = CurlOperationType.Long + 58,
		/// <summary>
		/// Proxy port to use.
		/// </summary>
		ProxyPort = CurlOperationType.Long + 59,

		/* size of the POST input data, if strlen() is not good to use */
		Postfieldsize = CurlOperationType.Long + 60,
		/// <summary>
		/// Tunnel through the HTTP proxy.
		/// </summary>
		HttpProxyTunnel = CurlOperationType.Long + 61,
		/// <summary>
		/// Bind connection locally to this.
		/// </summary>
		Interface = CurlOperationType.StringPointer + 62,

		/* Set the krb4/5 security level, this also enables krb4/5 awareness.  This
		 * is a string, 'clear', 'safe', 'confidential' or 'private'.  If the string
		 * is set but doesn't match one of these, 'private' will be used.  */
		Krblevel = CurlOperationType.StringPointer + 63,

		/* Set if we should verify the peer in ssl handshake, set 1 to verify. */
		SslVerifypeer = CurlOperationType.Long + 64,

		/* The CApath or CAfile used to validate the peer certificate
		   this option is used only if SSL_VERIFYPEER is true */
		Cainfo = CurlOperationType.StringPointer + 65,

		/* 66 = OBSOLETE */
		/* 67 = OBSOLETE */

		/* Maximum number of http redirects to follow */
		Maxredirs = CurlOperationType.Long + 68,

		/* Pass a long set to 1 to get the date of the requested document (if
		   possible)! Pass a zero to shut it off. */
		Filetime = CurlOperationType.Long + 69,

		/* This points to a linked list of telnet options */
		Telnetoptions = CurlOperationType.ObjectPointer + 70,

		/* Max amount of cached alive connections */
		Maxconnects = CurlOperationType.Long + 71,

		Obsolete72 = CurlOperationType.Long + 72, /* OBSOLETE, do not use! */

		/* 73 = OBSOLETE */

		/* Set to explicitly use a new connection for the upcoming transfer.
		   Do not use this unless you're absolutely sure of this, as it makes the
		   operation slower and is less friendly for the network. */
		FreshConnect = CurlOperationType.Long + 74,

		/* Set to explicitly forbid the upcoming transfer's connection to be re-used
		   when done. Do not use this unless you're absolutely sure of this, as it
		   makes the operation slower and is less friendly for the network. */
		ForbidReuse = CurlOperationType.Long + 75,

		/* Set to a file name that contains random data for libcurl to use to
		   seed the random engine when doing SSL connects. */
		RandomFile = CurlOperationType.StringPointer + 76,

		/* Set to the Entropy Gathering Daemon socket pathname */
		Egdsocket = CurlOperationType.StringPointer + 77,

		/* Time-out connect operations after this amount of seconds, if connects are
		   OK within this time, then fine... This only aborts the connect phase. */
		Connecttimeout = CurlOperationType.Long + 78,
		/// <summary>
		/// Callback for writing received headers.
		/// </summary>
		HeaderFunction = CurlOperationType.FunctionPointer + 79,

		/* Set this to force the HTTP request to get back to GET. Only really usable
		   if POST, PUT or a custom request have been used first.
		 */
		Httpget = CurlOperationType.Long + 80,

		/* Set if we should verify the Common name from the peer certificate in ssl
		 * handshake, set 1 to check existence, 2 to ensure that it matches the
		 * provided hostname. */
		SslVerifyhost = CurlOperationType.Long + 81,

		/* Specify which file name to write all known cookies in after completed
		   operation. Set file name to "-" (dash) to make it go to stdout. */
		Cookiejar = CurlOperationType.StringPointer + 82,

		/* Specify which SSL ciphers to use */
		SslCipherList = CurlOperationType.StringPointer + 83,

		/* Specify which HTTP version to use! This must be set to one of the
		   CURL_HTTP_VERSION* enums set below. */
		HttpVersion = CurlOperationType.Long + 84,

		/* Specifically switch on or off the FTP engine's use of the EPSV command. By
		   default, that one will always be attempted before the more traditional
		   PASV command. */
		FtpUseEpsv = CurlOperationType.Long + 85,

		/* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") */
		Sslcerttype = CurlOperationType.StringPointer + 86,

		/* name of the file keeping your private SSL-key */
		Sslkey = CurlOperationType.StringPointer + 87,

		/* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") */
		Sslkeytype = CurlOperationType.StringPointer + 88,

		/* crypto engine for the SSL-sub system */
		Sslengine = CurlOperationType.StringPointer + 89,

		/* set the crypto engine for the SSL-sub system as default
		   the param has no meaning...
		 */
		SslengineDefault = CurlOperationType.Long + 90,
		/// <summary>
		/// OBSOLETE Enable global DNS cache.
		/// </summary>
		DnsUseGlobalCache = CurlOperationType.Long + 91,
		/// <summary>
		/// Timeout for DNS cache.
		/// </summary>
		DnsCacheTimeout = CurlOperationType.Long + 92,

		/* send linked-list of pre-transfer QUOTE commands */
		Prequote = CurlOperationType.ObjectPointer + 93,
		/// <summary>
		/// Callback for debug information.
		/// </summary>
		DebugFunction = CurlOperationType.FunctionPointer + 94,
		/// <summary>
		/// Data pointer to pass to the debug callback.
		/// </summary>
		DebugData = CurlOperationType.ObjectPointer + 95,

		/* mark this as start of a cookie session */
		Cookiesession = CurlOperationType.Long + 96,

		/* The CApath directory used to validate the peer certificate
		   this option is used only if SSL_VERIFYPEER is true */
		Capath = CurlOperationType.StringPointer + 97,
		/// <summary>
		/// Ask for alternate buffer size.
		/// </summary>
		BufferSize = CurlOperationType.Long + 98,
		/// <summary>
		/// Do not install signal handlers.
		/// </summary>
		NoSignal = CurlOperationType.Long + 99,

		/* Provide a CURLShare for mutexing non-ts data */
		Share = CurlOperationType.ObjectPointer + 100,
		/// <summary>
		/// Proxy type.
		/// </summary>
		ProxyType = CurlOperationType.Long + 101,

		/* Set the Accept-Encoding string. Use this to tell a server you would like
		   the response to be compressed. Before 7.21.6, this was known as
		   CURLOPT_ENCODING */
		AcceptEncoding = CurlOperationType.StringPointer + 102,

		/* Set pointer to private data */
		Private = CurlOperationType.ObjectPointer + 103,

		/* Set aliases for HTTP 200 in the HTTP Response header */
		Http200Aliases = CurlOperationType.ObjectPointer + 104,

		/* Continue to send authentication (user+password) when following locations,
		   even when hostname changed. This can potentially send off the name
		   and password to whatever host the server decides. */
		UnrestrictedAuth = CurlOperationType.Long + 105,

		/* Specifically switch on or off the FTP engine's use of the EPRT command (
		   it also disables the LPRT attempt). By default, those ones will always be
		   attempted before the good old traditional PORT command. */
		FtpUseEprt = CurlOperationType.Long + 106,

		/* Set this to a bitmask value to enable the particular authentications
		   methods you like. Use this in combination with CURLOPT_USERPWD.
		   Note that setting multiple bits may cause extra network round-trips. */
		Httpauth = CurlOperationType.Long + 107,
		/// <summary>
		/// Callback for SSL context logic.
		/// </summary>
		SslCtxFunction = CurlOperationType.FunctionPointer + 108,
		/// <summary>
		/// Data pointer to pass to the SSL context callback.
		/// </summary>
		SslCtxData = CurlOperationType.ObjectPointer + 109,

		/* FTP Option that causes missing dirs to be created on the remote server.
		   In 7.19.4 we introduced the convenience enums for this option using the
		   CURLFTP_CREATE_DIR prefix.
		*/
		FtpCreateMissingDirs = CurlOperationType.Long + 110,

		/* Set this to a bitmask value to enable the particular authentications
		   methods you like. Use this in combination with CURLOPT_PROXYUSERPWD.
		   Note that setting multiple bits may cause extra network round-trips. */
		Proxyauth = CurlOperationType.Long + 111,

		/* FTP option that changes the timeout, in seconds, associated with
		   getting a response.  This is different from transfer timeout time and
		   essentially places a demand on the FTP server to acknowledge commands
		   in a timely manner. */
		FtpResponseTimeout = CurlOperationType.Long + 112,

		/* Set this option to one of the CURL_IPRESOLVE_* defines (see below) to
		   tell libcurl to resolve names to those IP versions only. This only has
		   affect on systems with support for more than one, i.e IPv4 _and_ IPv6. */
		Ipresolve = CurlOperationType.Long + 113,

		/* Set this option to limit the size of a file that will be downloaded from
		   an HTTP or FTP server.
		   Note there is also _LARGE version which adds large file support for
		   platforms which have larger off_t sizes.  See MAXFILESIZE_LARGE below. */
		Maxfilesize = CurlOperationType.Long + 114,

		/* See the comment for INFILESIZE above, but in short, specifies
		 * the size of the file being uploaded.  -1 means unknown.
		 */
		InfilesizeLarge = CurlOperationType.OffT + 115,

		/* Sets the continuation offset.  There is also a LONG version of this;
		 * look above for RESUME_FROM.
		 */
		ResumeFromLarge = CurlOperationType.OffT + 116,

		/* Sets the maximum size of data that will be downloaded from
		 * an HTTP or FTP server.  See MAXFILESIZE above for the LONG version.
		 */
		MaxfilesizeLarge = CurlOperationType.OffT + 117,

		/* Set this option to the file name of your .netrc file you want libcurl
		   to parse (using the CURLOPT_NETRC option). If not set, libcurl will do
		   a poor attempt to find the user's home directory and check for a .netrc
		   file in there. */
		NetrcFile = CurlOperationType.StringPointer + 118,

		/* Enable SSL/TLS for FTP, pick one of:
		   CURLUSESSL_TRY     - try using SSL, proceed anyway otherwise
		   CURLUSESSL_CONTROL - SSL for the control connection or fail
		   CURLUSESSL_ALL     - SSL for all communication or fail
		*/
		UseSsl = CurlOperationType.Long + 119,

		/* The _LARGE version of the standard POSTFIELDSIZE option */
		PostfieldsizeLarge = CurlOperationType.OffT + 120,
		/// <summary>
		/// Disable the Nagle algorithm.
		/// </summary>
		TcpNodelay = CurlOperationType.Long + 121,

		/* 122 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
		/* 123 OBSOLETE. Gone in 7.16.0 */
		/* 124 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
		/* 125 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
		/* 126 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
		/* 127 OBSOLETE. Gone in 7.16.0 */
		/* 128 OBSOLETE. Gone in 7.16.0 */

		/* When FTP over SSL/TLS is selected (with CURLOPT_USE_SSL), this option
		   can be used to change libcurl's default action which is to first try
		   "AUTH SSL" and then "AUTH TLS" in this order, and proceed when a OK
		   response has been received.
		   Available parameters are:
		   CURLFTPAUTH_DEFAULT - let libcurl decide
		   CURLFTPAUTH_SSL     - try "AUTH SSL" first, then TLS
		   CURLFTPAUTH_TLS     - try "AUTH TLS" first, then SSL
		*/
		Ftpsslauth = CurlOperationType.Long + 129,
		/// <summary>
		/// Callback for I/O operations.
		/// </summary>
		IoctlFunction = CurlOperationType.FunctionPointer + 130,
		/// <summary>
		/// Data pointer to pass to the I/O callback.
		/// </summary>
		IoctlData = CurlOperationType.ObjectPointer + 131,

		/* 132 OBSOLETE. Gone in 7.16.0 */
		/* 133 OBSOLETE. Gone in 7.16.0 */

		/* zero terminated string for pass on to the FTP server when asked for
		   "account" info */
		FtpAccount = CurlOperationType.StringPointer + 134,

		/* feed cookie into cookie engine */
		Cookielist = CurlOperationType.StringPointer + 135,

		/* ignore Content-Length */
		IgnoreContentLength = CurlOperationType.Long + 136,

		/* Set to non-zero to skip the IP address received in a 227 PASV FTP server
		   response. Typically used for FTP-SSL purposes but is not restricted to
		   that. libcurl will then instead use the same IP address it used for the
		   control connection. */
		FtpSkipPasvIp = CurlOperationType.Long + 137,

		/* Select "file method" to use when doing FTP, see the curl_ftpmethod
		   above. */
		FtpFilemethod = CurlOperationType.Long + 138,
		/// <summary>
		/// Bind connection locally to this port.
		/// </summary>
		LocalPort = CurlOperationType.Long + 139,
		/// <summary>
		/// Bind connection locally to port range.
		/// </summary>
		LocalPortRange = CurlOperationType.Long + 140,

		/* no transfer, set up connection and let application use the socket by
		   extracting it with CURLINFO_LASTSOCKET */
		ConnectOnly = CurlOperationType.Long + 141,
		/// <summary>
		/// Callback for code base conversion.
		/// </summary>
		ConvFromNetworkFunction = CurlOperationType.FunctionPointer + 142,
		/// <summary>
		/// Callback for code base conversion.
		/// </summary>
		ConvToNetworkFunction = CurlOperationType.FunctionPointer + 143,
		/// <summary>
		/// Callback for code base conversion.
		/// </summary>
		ConvFromUtf8Function = CurlOperationType.FunctionPointer + 144,

		/* if the connection proceeds too quickly then need to slow it down */
		/* limit-rate: maximum number of bytes per second to send or receive */
		MaxSendSpeedLarge = CurlOperationType.OffT + 145,
		MaxRecvSpeedLarge = CurlOperationType.OffT + 146,

		/* Pointer to command string to send if USER/PASS fails. */
		FtpAlternativeToUser = CurlOperationType.StringPointer + 147,
		/// <summary>
		/// Callback for sockopt operations.
		/// </summary>
		SockOptFunction = CurlOperationType.FunctionPointer + 148,
		/// <summary>
		/// Data pointer to pass to the sockopt callback.
		/// </summary>
		SockOptData = CurlOperationType.ObjectPointer + 149,

		/* set to 0 to disable session ID re-use for this transfer, default is
		   enabled (== 1) */
		SslSessionidCache = CurlOperationType.Long + 150,

		/* allowed SSH authentication methods */
		SshAuthTypes = CurlOperationType.Long + 151,

		/* Used by scp/sftp to do public/private key authentication */
		SshPublicKeyfile = CurlOperationType.StringPointer + 152,
		SshPrivateKeyfile = CurlOperationType.StringPointer + 153,

		/* Send CCC (Clear Command Channel) after authentication */
		FtpSslCcc = CurlOperationType.Long + 154,

		/* Same as TIMEOUT and CONNECTTIMEOUT, but with ms resolution */
		TimeoutMs = CurlOperationType.Long + 155,
		ConnecttimeoutMs = CurlOperationType.Long + 156,

		/* set to zero to disable the libcurl's decoding and thus pass the raw body
		   data to the application even when it is encoded/compressed */
		HttpTransferDecoding = CurlOperationType.Long + 157,
		HttpContentDecoding = CurlOperationType.Long + 158,

		/* Permission used when creating new files and directories on the remote
		   server for protocols that support it, SFTP/SCP/FILE */
		NewFilePerms = CurlOperationType.Long + 159,
		NewDirectoryPerms = CurlOperationType.Long + 160,

		/* Set the behaviour of POST when redirecting. Values must be set to one
		   of CURL_REDIR* defines below. This used to be called CURLOPT_POST301 */
		Postredir = CurlOperationType.Long + 161,

		/* used by scp/sftp to verify the host's public key */
		SshHostPublicKeyMd5 = CurlOperationType.StringPointer + 162,
		/// <summary>
		/// Callback for socket creation.
		/// </summary>
		OpenSocketFunction = CurlOperationType.FunctionPointer + 163,
		/// <summary>
		/// Data pointer to pass to the open socket callback.
		/// </summary>
		OpenSocketData = CurlOperationType.ObjectPointer + 164,

		/* POST volatile input fields. */
		Copypostfields = CurlOperationType.ObjectPointer + 165,

		/* set transfer mode (;type=<a|i>) when doing FTP via an HTTP proxy */
		ProxyTransferMode = CurlOperationType.Long + 166,
		/// <summary>
		/// Callback for seek operations.
		/// </summary>
		SeekFunction = CurlOperationType.FunctionPointer + 167,
		/// <summary>
		/// Data pointer to pass to the seek callback.
		/// </summary>
		SeekData = CurlOperationType.ObjectPointer + 168,

		/* CRL file */
		Crlfile = CurlOperationType.StringPointer + 169,

		/* Issuer certificate */
		Issuercert = CurlOperationType.StringPointer + 170,
		/// <summary>
		/// IPv6 scope for local addresses.
		/// </summary>
		AddressScope = CurlOperationType.Long + 171,

		/* Collect certificate chain info and allow it to get retrievable with
		   CURLINFO_CERTINFO after the transfer is complete. */
		Certinfo = CurlOperationType.Long + 172,

		/* "name" and "pwd" to use when fetching. */
		Username = CurlOperationType.StringPointer + 173,
		Password = CurlOperationType.StringPointer + 174,

		/* "name" and "pwd" to use with Proxy when fetching. */
		Proxyusername = CurlOperationType.StringPointer + 175,
		Proxypassword = CurlOperationType.StringPointer + 176,
		/// <summary>
		/// Filter out hosts from proxy use.
		/// </summary>
		NoProxy = CurlOperationType.StringPointer + 177,

		/* block size for TFTP transfers */
		TftpBlksize = CurlOperationType.Long + 178,
		/// <summary>
		/// Socks5 GSSAPI service name.
		/// </summary>
		Socks5GssapiService = CurlOperationType.StringPointer + 179,
		/// <summary>
		/// Socks5 GSSAPI NEC mode.
		/// </summary>
		Socks5GssapiNec = CurlOperationType.Long + 180,
		/// <summary>
		/// Allowed protocols.
		/// </summary>
		Protocols = CurlOperationType.Long + 181,
		/// <summary>
		/// Protocols to allow redirects to.
		/// </summary>
		RedirProtocols = CurlOperationType.Long + 182,

		/* set the SSH knownhost file name to use */
		SshKnownhosts = CurlOperationType.StringPointer + 183,

		/* set the SSH host key callback, must point to a curl_sshkeycallback
		   function */
		SshKeyfunction = CurlOperationType.FunctionPointer + 184,

		/* set the SSH host key callback custom pointer */
		SshKeydata = CurlOperationType.ObjectPointer + 185,

		/* set the SMTP mail originator */
		MailFrom = CurlOperationType.StringPointer + 186,

		/* set the list of SMTP mail receiver(s) */
		MailRcpt = CurlOperationType.ObjectPointer + 187,

		/* FTP: send PRET before PASV */
		FtpUsePret = CurlOperationType.Long + 188,

		/* RTSP request method (OPTIONS, SETUP, PLAY, etc...) */
		RtspRequest = CurlOperationType.Long + 189,

		/* The RTSP session identifier */
		RtspSessionId = CurlOperationType.StringPointer + 190,

		/* The RTSP stream URI */
		RtspStreamUri = CurlOperationType.StringPointer + 191,

		/* The Transport: header to use in RTSP requests */
		RtspTransport = CurlOperationType.StringPointer + 192,

		/* Manually initialize the client RTSP CSeq for this handle */
		RtspClientCseq = CurlOperationType.Long + 193,

		/* Manually initialize the server RTSP CSeq for this handle */
		RtspServerCseq = CurlOperationType.Long + 194,
		/// <summary>
		/// Data pointer to pass to the RTSP interleave callback.
		/// </summary>
		InterleaveData = CurlOperationType.ObjectPointer + 195,
		/// <summary>
		/// Callback for RTSP interleaved data.
		/// </summary>
		InterleaveFunction = CurlOperationType.FunctionPointer + 196,
		/// <summary>
		/// Transfer multiple files according to a file name pattern.
		/// </summary>
		WildcardMatch = CurlOperationType.Long + 197,
		/// <summary>
		/// Callback for wildcard download start of chunk.
		/// </summary>
		ChunkBgnFunction = CurlOperationType.FunctionPointer + 198,
		/// <summary>
		/// Callback for wildcard download end of chunk.
		/// </summary>
		ChunkEndFunction = CurlOperationType.FunctionPointer + 199,
		/// <summary>
		/// Callback for wildcard matching.
		/// </summary>
		FnmatchFunction = CurlOperationType.FunctionPointer + 200,
		/// <summary>
		/// Data pointer to pass to the chunk callbacks
		/// </summary>
		ChunkData = CurlOperationType.ObjectPointer + 201,
		/// <summary>
		/// Data pointer to pass to the wildcard matching callback.
		/// </summary>
		FnmatchData = CurlOperationType.ObjectPointer + 202,

		/* send linked-list of name:port:address sets */
		Resolve = CurlOperationType.ObjectPointer + 203,

		/* Set a username for authenticated TLS */
		TlsauthUsername = CurlOperationType.StringPointer + 204,

		/* Set a password for authenticated TLS */
		TlsauthPassword = CurlOperationType.StringPointer + 205,

		/* Set authentication type for authenticated TLS */
		TlsauthType = CurlOperationType.StringPointer + 206,

		/* Set to 1 to enable the "TE:" header in HTTP requests to ask for
		   compressed transfer-encoded responses. Set to 0 to disable the use of TE:
		   in outgoing requests. The current default is 0, but it might change in a
		   future libcurl release.
		   libcurl will ask for the compressed methods it knows of, and if that
		   isn't any, it will not ask for transfer-encoding at all even if this
		   option is set to 1.
		*/
		TransferEncoding = CurlOperationType.Long + 207,
		/// <summary>
		/// Callback for closing socket.
		/// </summary>
		CloseSocketFunction = CurlOperationType.FunctionPointer + 208,
		/// <summary>
		/// Data pointer to pass to the close socket callback.
		/// </summary>
		CloseSocketData = CurlOperationType.ObjectPointer + 209,

		/* allow GSSAPI credential delegation */
		GssapiDelegation = CurlOperationType.Long + 210,

		/* Set the name servers to use for DNS resolution */
		DnsServers = CurlOperationType.StringPointer + 211,

		/* Time-out accept operations (currently for FTP only) after this amount
		   of milliseconds. */
		AccepttimeoutMs = CurlOperationType.Long + 212,
		/// <summary>
		/// Enable TCP keep-alive.
		/// </summary>
		TcpKeepAlive = CurlOperationType.Long + 213,
		/// <summary>
		/// Idle time before sending keep-alive.
		/// </summary>
		TcpKeepIdle = CurlOperationType.Long + 214,
		/// <summary>
		/// Interval between keep-alive probes.
		/// </summary>
		TcpKeepIntvl = CurlOperationType.Long + 215,

		/* Enable/disable specific SSL features with a bitmask, see CURLSSLOPT_* */
		SslOptions = CurlOperationType.Long + 216,

		/* Set the SMTP auth originator */
		MailAuth = CurlOperationType.StringPointer + 217,

		/* Enable/disable SASL initial response */
		SaslIr = CurlOperationType.Long + 218,
		/// <summary>
		/// Callback for progress meter.
		/// </summary>
		XferInfoFunction = CurlOperationType.FunctionPointer + 219,
		/// <summary>
		/// Data pointer to pass to the progress meter callback.
		/// </summary>
		XferInfoData = ProgressData,

		/* The XOAUTH2 bearer token */
		Xoauth2Bearer = CurlOperationType.StringPointer + 220,

		/* Set the interface string to use as outgoing network
		 * interface for DNS requests.
		 * Only supported by the c-ares DNS backend */
		DnsInterface = CurlOperationType.StringPointer + 221,

		/* Set the local IPv4 address to use for outgoing DNS requests.
		 * Only supported by the c-ares DNS backend */
		DnsLocalIp4 = CurlOperationType.StringPointer + 222,

		/* Set the local IPv4 address to use for outgoing DNS requests.
		 * Only supported by the c-ares DNS backend */
		DnsLocalIp6 = CurlOperationType.StringPointer + 223,

		/* Set authentication options directly */
		LoginOptions = CurlOperationType.StringPointer + 224,

		/* Enable/disable TLS NPN extension (http2 over ssl might fail without) */
		SslEnableNpn = CurlOperationType.Long + 225,

		/* Enable/disable TLS ALPN extension (http2 over ssl might fail without) */
		SslEnableAlpn = CurlOperationType.Long + 226,

		/* Time to wait for a response to a HTTP request containing an
		 * Expect: 100-continue header before sending the data anyway. */
		Expect100TimeoutMs = CurlOperationType.Long + 227,

		/* This points to a linked list of headers used for proxy requests only,
		   struct curl_slist kind */
		Proxyheader = CurlOperationType.ObjectPointer + 228,

		/* Pass in a bitmask of "header options" */
		Headeropt = CurlOperationType.Long + 229,

		/* The public key in DER form used to validate the peer public key
		   this option is used only if SSL_VERIFYPEER is true */
		Pinnedpublickey = CurlOperationType.StringPointer + 230,
		/// <summary>
		/// Path to a Unix domain socket.
		/// </summary>
		UnixSocketPath = CurlOperationType.StringPointer + 231,

		/* Set if we should verify the certificate status. */
		SslVerifystatus = CurlOperationType.Long + 232,

		/* Set if we should enable TLS false start. */
		SslFalsestart = CurlOperationType.Long + 233,
		/// <summary>
		/// Disable squashing /../ and /./ sequences in the path.
		/// </summary>
		PathAsIs = CurlOperationType.Long + 234,
		/// <summary>
		/// Proxy authentication service name.
		/// </summary>
		ProxyServiceName = CurlOperationType.StringPointer + 235,
		/// <summary>
		/// Authentication service name.
		/// </summary>
		ServiceName = CurlOperationType.StringPointer + 236,

		/* Wait/don't wait for pipe/mutex to clarify */
		Pipewait = CurlOperationType.Long + 237,
		/// <summary>
		/// Default protocol.
		/// </summary>
		DefaultProtocol = CurlOperationType.StringPointer + 238,

		/* Set stream weight, 1 - 256 (default is 16) */
		StreamWeight = CurlOperationType.Long + 239,

		/* Set stream dependency on another CURL handle */
		StreamDepends = CurlOperationType.ObjectPointer + 240,

		/* Set E-xclusive stream dependency on another CURL handle */
		StreamDependsE = CurlOperationType.ObjectPointer + 241,

		/* Do not send any tftp option requests to the server */
		TftpNoOptions = CurlOperationType.Long + 242,
		/// <summary>
		/// Connect to a specific host and port.
		/// </summary>
		ConnectTo = CurlOperationType.ObjectPointer + 243,
		/// <summary>
		/// Enable TFO, TCP Fast Open.
		/// </summary>
		TcpFastOpen = CurlOperationType.Long + 244,
		/// <summary>
		/// Keep sending on HTTP >= 300 errors.
		/// </summary>
		KeepSendingOnError = CurlOperationType.Long + 245,

		/* The CApath or CAfile used to validate the proxy certificate
		   this option is used only if PROXY_SSL_VERIFYPEER is true */
		ProxyCainfo = CurlOperationType.StringPointer + 246,

		/* The CApath directory used to validate the proxy certificate
		   this option is used only if PROXY_SSL_VERIFYPEER is true */
		ProxyCapath = CurlOperationType.StringPointer + 247,

		/* Set if we should verify the proxy in ssl handshake,
		   set 1 to verify. */
		ProxySslVerifypeer = CurlOperationType.Long + 248,

		/* Set if we should verify the Common name from the proxy certificate in ssl
		 * handshake, set 1 to check existence, 2 to ensure that it matches
		 * the provided hostname. */
		ProxySslVerifyhost = CurlOperationType.Long + 249,

		/* What version to specifically try to use for proxy.
		   See CURL_SSLVERSION defines below. */
		ProxySslversion = CurlOperationType.Long + 250,

		/* Set a username for authenticated TLS for proxy */
		ProxyTlsauthUsername = CurlOperationType.StringPointer + 251,

		/* Set a password for authenticated TLS for proxy */
		ProxyTlsauthPassword = CurlOperationType.StringPointer + 252,

		/* Set authentication type for authenticated TLS for proxy */
		ProxyTlsauthType = CurlOperationType.StringPointer + 253,

		/* name of the file keeping your private SSL-certificate for proxy */
		ProxySslcert = CurlOperationType.StringPointer + 254,

		/* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") for
		   proxy */
		ProxySslcerttype = CurlOperationType.StringPointer + 255,

		/* name of the file keeping your private SSL-key for proxy */
		ProxySslkey = CurlOperationType.StringPointer + 256,

		/* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") for
		   proxy */
		ProxySslkeytype = CurlOperationType.StringPointer + 257,

		/* password for the SSL private key for proxy */
		ProxyKeypasswd = CurlOperationType.StringPointer + 258,

		/* Specify which SSL ciphers to use for proxy */
		ProxySslCipherList = CurlOperationType.StringPointer + 259,

		/* CRL file for proxy */
		ProxyCrlfile = CurlOperationType.StringPointer + 260,

		/* Enable/disable specific SSL features with a bitmask for proxy, see
		   CURLSSLOPT_* */
		ProxySslOptions = CurlOperationType.Long + 261,
		/// <summary>
		/// Socks proxy to use.
		/// </summary>
		PreProxy = CurlOperationType.StringPointer + 262,

		/* The public key in DER form used to validate the proxy public key
		   this option is used only if PROXY_SSL_VERIFYPEER is true */
		ProxyPinnedpublickey = CurlOperationType.StringPointer + 263,
		/// <summary>
		/// Path to an abstract Unix domain socket.
		/// </summary>
		AbstractUnixSocket = CurlOperationType.StringPointer + 264,
		/// <summary>
		/// Suppress proxy CONNECT response headers from user callbacks.
		/// </summary>
		SuppressConnectHeaders = CurlOperationType.Long + 265,
		/// <summary>
		/// Socks5 authentication methods.
		/// </summary>
		Socks5Auth = CurlOperationType.Long + 267,
		/// <summary>
		/// Callback to be called before a new resolve request is started.
		/// </summary>
		ResolverStartFunction = CurlOperationType.FunctionPointer + 272,
		/// <summary>
		/// Data pointer to pass to resolver start callback.
		/// </summary>
		ResolverStartData = CurlOperationType.ObjectPointer + 273,
		/// <summary>
		/// Send an HAProxy PROXY protocol v1 header.
		/// </summary>
		HaProxyProtocol = CurlOperationType.Long + 274,
		/// <summary>
		/// Use this DOH server for name resolves.
		/// </summary>
		DohUrl = CurlOperationType.StringPointer + 279,

		CurloptLastentry /* the last unused */
	}
}
