namespace CurlNet.Enums
{
	internal enum CurlInfo : uint
	{
		/// <summary>
		/// Not specified.
		/// </summary>
		None = 0,
		/// <summary>
		/// Last used URL.
		/// </summary>
		EffectiveUrl = CurlInfoType.String + 1,
		/// <summary>
		/// Last received response code.
		/// </summary>
		ResponseCode = CurlInfoType.Long + 2,
		/// <summary>
		/// Total time of previous transfer.
		/// </summary>
		TotalTime = CurlInfoType.Double + 3,
		/// <summary>
		/// Time from start until name resolving completed.
		/// </summary>
		NameLookupTime = CurlInfoType.Double + 4,
		/// <summary>
		/// Time from start until remote host or proxy completed.
		/// </summary>
		ConnectTime = CurlInfoType.Double + 5,
		/// <summary>
		/// Time from start until just before the transfer begins.
		/// </summary>
		PretransferTime = CurlInfoType.Double + 6,
		/// <summary>
		/// (Deprecated) Number of bytes uploaded.
		/// </summary>
		SizeUpload = CurlInfoType.Double + 7,
		/// <summary>
		/// Number of bytes uploaded.
		/// </summary>
		SizeUploadT = CurlInfoType.OffT + 7,
		/// <summary>
		/// (Deprecated) Number of bytes downloaded.
		/// </summary>
		SizeDownload = CurlInfoType.Double + 8,
		/// <summary>
		/// Number of bytes downloaded.
		/// </summary>
		SizeDownloadT = CurlInfoType.OffT + 8,
		/// <summary>
		/// (Deprecated) Average download speed.
		/// </summary>
		SpeedDownload = CurlInfoType.Double + 9,
		/// <summary>
		/// Average download speed.
		/// </summary>
		SpeedDownloadT = CurlInfoType.OffT + 9,
		/// <summary>
		/// (Deprecated) Average upload speed.
		/// </summary>
		SpeedUpload = CurlInfoType.Double + 10,
		/// <summary>
		/// Average upload speed.
		/// </summary>
		SpeedUploadT = CurlInfoType.OffT + 10,
		/// <summary>
		/// Number of bytes of all headers received.
		/// </summary>
		HeaderSize = CurlInfoType.Long + 11,
		/// <summary>
		/// Number of bytes sent in the issued HTTP requests.
		/// </summary>
		RequestSize = CurlInfoType.Long + 12,
		/// <summary>
		/// Certificate verification result.
		/// </summary>
		SslVerifyResult = CurlInfoType.Long + 13,
		/// <summary>
		/// Remote time of the retrieved document.
		/// </summary>
		FileTime = CurlInfoType.Long + 14,
		/// <summary>
		/// Remote time of the retrieved document.
		/// </summary>
		FileTimeT = CurlInfoType.OffT + 14,
		/// <summary>
		/// (Deprecated) Content length from the Content-Length header.
		/// </summary>
		ContentLengthDownload = CurlInfoType.Double + 15,
		/// <summary>
		/// Content length from the Content-Length header.
		/// </summary>
		ContentLengthDownloadT = CurlInfoType.OffT + 15,
		/// <summary>
		/// (Deprecated) Upload size.
		/// </summary>
		ContentLengthUpload = CurlInfoType.Double + 16,
		/// <summary>
		/// Upload size.
		/// </summary>
		ContentLengthUploadT = CurlInfoType.OffT + 16,
		/// <summary>
		/// Time from start until just when the first byte is received.
		/// </summary>
		StartTransferTime = CurlInfoType.Double + 17,
		/// <summary>
		/// Content type from the Content-Type header.
		/// </summary>
		ContentType = CurlInfoType.String + 18,
		/// <summary>
		/// Time taken for all redirect steps before the final transfer.
		/// </summary>
		RedirectTime = CurlInfoType.Double + 19,
		/// <summary>
		/// Total number of redirects that were followed.
		/// </summary>
		RedirectCount = CurlInfoType.Long + 20,
		/// <summary>
		/// User's private data pointer.
		/// </summary>
		Private = CurlInfoType.String + 21,
		/// <summary>
		/// Last proxy CONNECT response code.
		/// </summary>
		HttpConnectCode = CurlInfoType.Long + 22,
		/// <summary>
		/// Available HTTP authentication methods.
		/// </summary>
		HttpAuthAvail = CurlInfoType.Long + 23,
		/// <summary>
		/// Available HTTP proxy authentication methods.
		/// </summary>
		ProxyAuthAvail = CurlInfoType.Long + 24,
		/// <summary>
		/// The errno from the last failure to connect.
		/// </summary>
		OsErrno = CurlInfoType.Long + 25,
		/// <summary>
		/// Number of new successful connections used for previous transfer.
		/// </summary>
		NumConnects = CurlInfoType.Long + 26,
		/// <summary>
		/// A list of OpenSSL crypto engines.
		/// </summary>
		SslEngines = CurlInfoType.Slist + 27,
		/// <summary>
		/// List of all known cookies.
		/// </summary>
		CookieList = CurlInfoType.Slist + 28,
		/// <summary>
		/// Last socket used.
		/// </summary>
		LastSocket = CurlInfoType.Long + 29,
		/// <summary>
		/// The entry path after logging in to an FTP server.
		/// </summary>
		FtpEntryPath = CurlInfoType.String + 30,
		/// <summary>
		/// URL a redirect would take you to, had you enabled redirects.
		/// </summary>
		RedirectUrl = CurlInfoType.String + 31,
		/// <summary>
		/// IP address of the last connection.
		/// </summary>
		PrimaryIp = CurlInfoType.String + 32,
		/// <summary>
		/// Time from start until SSL/SSH handshake completed.
		/// </summary>
		AppConnectTime = CurlInfoType.Double + 33,
		/// <summary>
		/// Certificate chain.
		/// </summary>
		CertInfo = CurlInfoType.Pointer + 34,
		/// <summary>
		/// Whether or not a time conditional was met.
		/// </summary>
		ConditionUnmet = CurlInfoType.Long + 35,
		/// <summary>
		/// RTSP session ID.
		/// </summary>
		RtspSessionId = CurlInfoType.String + 36,
		/// <summary>
		/// RTSP CSeq that will next be used.
		/// </summary>
		RtspClientCseq = CurlInfoType.Long + 37,
		/// <summary>
		/// RTSP CSeq that will next be expected.
		/// </summary>
		RtspServerCseq = CurlInfoType.Long + 38,
		/// <summary>
		/// RTSP CSeq last received.
		/// </summary>
		RtspCseqRecv = CurlInfoType.Long + 39,
		/// <summary>
		/// Port of the last connection.
		/// </summary>
		PrimaryPort = CurlInfoType.Long + 40,
		/// <summary>
		/// Local-end IP address of last connection.
		/// </summary>
		LocalIp = CurlInfoType.String + 41,
		/// <summary>
		/// Local-end port of last connection.
		/// </summary>
		LocalPort = CurlInfoType.Long + 42,
		/// <summary>
		/// TLS session info that can be used for further processing. Deprecated option, use TlsSslPointer instead!
		/// </summary>
		TlsSession = CurlInfoType.Pointer + 43,
		/// <summary>
		/// The session's active socket.
		/// </summary>
		ActiveSocket = CurlInfoType.Socket + 44,
		/// <summary>
		/// TLS session info that can be used for further processing.
		/// </summary>
		TlsSslPointer = CurlInfoType.Pointer + 45,
		/// <summary>
		/// The http version used in the connection.
		/// </summary>
		HttpVersion = CurlInfoType.Long + 46,
		/// <summary>
		/// Proxy certificate verification result.
		/// </summary>
		ProxySslVerifyResult = CurlInfoType.Long + 47,
		/// <summary>
		/// The protocol used for the connection.
		/// </summary>
		Protocol = CurlInfoType.Long + 48,
		/// <summary>
		/// The scheme used for the connection.
		/// </summary>
		Scheme = CurlInfoType.String + 49,
		/// <summary>
		/// Total time of previous transfer.
		/// </summary>
		TotalTimeT = CurlInfoType.OffT + 50,
		/// <summary>
		/// Time from start until name resolving completed.
		/// </summary>
		NameLookupTimeT = CurlInfoType.OffT + 51,
		/// <summary>
		/// Time from start until remote host or proxy completed.
		/// </summary>
		ConnectTimeT = CurlInfoType.OffT + 52,
		/// <summary>
		/// Time from start until just before the transfer begins.
		/// </summary>
		PretransferTimeT = CurlInfoType.OffT + 53,
		/// <summary>
		/// Time from start until just when the first byte is received.
		/// </summary>
		StartTransferTimeT = CurlInfoType.OffT + 54,
		/// <summary>
		/// Time taken for all redirect steps before the final transfer.
		/// </summary>
		RedirectTimeT = CurlInfoType.OffT + 55,
		/// <summary>
		/// Time from start until SSL/SSH handshake completed.
		/// </summary>
		AppConnectTimeT = CurlInfoType.OffT + 56,
		/// <summary>
		/// The value from the from the Retry-After header.
		/// </summary>
		RetryAfter = CurlInfoType.OffT + 57,

		LastOne = 57
	}
}