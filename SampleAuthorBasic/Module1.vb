Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading.Tasks

Module Module1
    Sub Main()
        Dim shop As String = "https://lublue7.myharavan.com/admin"

        Dim apiKey As String = "a75c7d8bd76e08ee03344a21cc0b0b45"

        Dim password As String = "5ce968fcb49bc90e9d89f6dd7279c447"

        Dim token = HashToToken(apiKey, password)

        Dim result = SendRequest(shop + "/products.json", HttpMethod.Get, token).Result

    End Sub

    Public Function HashToToken(ByVal apiKey As String, ByVal password As String) As String
        Dim authbytes As Byte() = Encoding.ASCII.GetBytes($"{apiKey}:{password}")
        Return Convert.ToBase64String(authbytes)
    End Function

    Private Async Function SendRequest(ByVal url As String, ByVal method As HttpMethod, ByVal access_token As String, ByVal Optional data As String = Nothing) As Task(Of String)
        Dim rq = New HttpRequestMessage(method, url)
        rq.Headers.Authorization = New AuthenticationHeaderValue("Basic", access_token)
        rq.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        If data IsNot Nothing Then rq.Content = New StringContent(data, Encoding.UTF8, "application/json")

        Dim result As String = ""

        Using client As HttpClient = New HttpClient()
            Using response As HttpResponseMessage = Await client.SendAsync(rq)
                Using content As HttpContent = response.Content
                    ' Get contents of page as a String.
                    result = Await content.ReadAsStringAsync()

                End Using
            End Using
        End Using

        Return result
    End Function
End Module
