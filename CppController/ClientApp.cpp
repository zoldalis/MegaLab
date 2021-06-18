#define WIN32_LEAN_AND_MEAN
#include <iostream>
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include <cstdlib>
#include "objbase.h"
//#include "iostream.h"
using namespace std;

GUID gidReference;
HRESULT hCreateGuid = CoCreateGuid( &gidReference );
// Need to link with Ws2_32.lib, Mswsock.lib, and Advapi32.lib
#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")


#define DEFAULT_BUFLEN 1024
#define DEFAULT_PORT "4040"

int main()
{
    using std::string;
    setlocale(LC_ALL, "Russian");
    // КОНТРОЛЛЕР ТЕМПЕРАТУРЫ (Контроллер температуры имеет следующие настройки: температура начала/окончания проветривания, температура 
    //начала/окончания подогрева, верхняя и нижняя предельная граница температур для отправки экстренных смс сообщений и номер телефона для отправки сообщений.
    Sleep(10000);
    WSADATA wsaData;
    SOCKET ConnectSocket = INVALID_SOCKET;
    struct addrinfo* result = NULL,
        * ptr = NULL,
        hints;
    char mes[1024];
    int T = 23;
    int TsA = 27;
    int TeA = 17;
    int TsH = 10;
    int TeH = 20;
    int TH = 30;
    int TL = 9;

LPOLESTR szGUID = new WCHAR[39];
    HRESULT hr;
    GUID  guid;
    hr = CoCreateGuid(&guid);
    if (!FAILED(hr))
    {
        
        StringFromGUID2(guid, szGUID, 39);
        wprintf(L"GUID: %s", szGUID);
        cout << szGUID << endl;
    }
    wcout << szGUID << endl;


    std::string sendbuf = "rtyuiop";
    //std::cin >> sendbuf;
    char recvbuf[DEFAULT_BUFLEN];
    int iResult;
    int recvbuflen = DEFAULT_BUFLEN;



    // Initialize Winsock
    iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (iResult != 0) {
        printf("WSAStartup failed with error: %d\n", iResult);
        return 1;
    }

    ZeroMemory(&hints, sizeof(hints));
    hints.ai_family = AF_UNSPEC;
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_protocol = IPPROTO_TCP;

    // Resolve the server address and port
    iResult = getaddrinfo("127.0.0.1", DEFAULT_PORT, &hints, &result);
    if (iResult != 0) {
        printf("getaddrinfo failed with error: %d\n", iResult);
        WSACleanup();
        return 1;
    }

    // Attempt to connect to an address until one succeeds
    for (ptr = result; ptr != NULL; ptr = ptr->ai_next) {

        // Create a SOCKET for connecting to server
        ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype,
            ptr->ai_protocol);
        if (ConnectSocket == INVALID_SOCKET) {
            printf("socket failed with error: %ld\n", WSAGetLastError());
            WSACleanup();
            return 1;
        }

        // Connect to server.
        iResult = connect(ConnectSocket, ptr->ai_addr, (int)ptr->ai_addrlen);
        if (iResult == SOCKET_ERROR) {
            closesocket(ConnectSocket);
            ConnectSocket = INVALID_SOCKET;
            continue;
        }
        break;
    }

    freeaddrinfo(result);

    if (ConnectSocket == INVALID_SOCKET) {
        printf("Unable to connect to server!\n");
        WSACleanup();
        return 1;
    }

    // Send an initial buffer
    iResult = send(ConnectSocket, sendbuf.c_str(), (int)strlen(sendbuf.c_str()), 0);
    if (iResult == SOCKET_ERROR) {
        printf("send failed with error: %d\n", WSAGetLastError());
        closesocket(ConnectSocket);
        WSACleanup();
        return 1;
    }
    T = rand() % 13 + 10;
    printf("Bytes Sent: %ld\n", iResult);
    bool Flag = false;
    bool Flag2 = false;
    for (int i = 0; i < 10; i++)
    {


       
        if (T >= TsA && T<TH)
        {
            Flag = true;
            cout << "Начало проветривания " << T << endl;
            T = T - rand() % 20 + 1;
            goto asd;
        }
        else if (T < TsA && Flag == true && T> TeA)
        {
            cout << "Продолжается проветривание " << T << endl;
            T = T - rand() % 17 + 1;
            goto asd;
        }
        else if (T <= TeA && Flag == true)
        {
            cout << "Проветривание закончено " << T << endl;
            Flag = false;
            T = rand() % 10 + 1;
            goto asd;
        }

        if (T <= TsH && T> TL)
        {
            cout << "Начало подогрева " << T << endl;
            Flag2 = true;
            T = rand() % 15 + T;
            goto asd;
        }
        if (T > TsH && Flag2 == true && T<TeH)
        {
            cout << "Продолжается подогрев " << T << endl;
            T = rand() % 10 + T;
            goto asd;
        }
        if (T >= TeH && Flag2 == true)
        {
            Flag2 = false;
            cout << "Подогрев закончен " << T << endl;
            T = rand() % 10 + T;
            goto asd;
        }
        if (T >= TH)
        {
            cout << "Предельно большая температура, отправка смс " << T << endl;
            T = rand() % 10 + 10;
            goto asd;
        }
        if (T <= TL)
        {
            cout << "Предельно низкая теспература, отправка смс " << T << endl;
            T = rand() % 15 + 15;
            goto asd;
        }
        if (T < 27 && T>10)
        {
            cout << "Нормальная температура " << T << endl;
            
        }
        asd:
        sendbuf = T;
        iResult = send(ConnectSocket, sendbuf.c_str(), (int)strlen(sendbuf.c_str()), 0);
        if (iResult == SOCKET_ERROR) {
            printf("send failed with error: %d\n", WSAGetLastError());
            closesocket(ConnectSocket);
            WSACleanup();
            return 1;
        }

        printf("Bytes Sent: %ld\n", iResult);
        T = T + 5;
    }



    // shutdown the connection since no more data will be sent
    iResult = shutdown(ConnectSocket, SD_SEND);
    if (iResult == SOCKET_ERROR) {
        printf("shutdown failed with error: %d\n", WSAGetLastError());
        closesocket(ConnectSocket);
        WSACleanup();
        return 1;
    }

    // Receive until the peer closes the connection
    do {

        iResult = recv(ConnectSocket, recvbuf, recvbuflen, 0);
        if (iResult > 0)
        {
            //std::string s(static_cast<char const*>(recvbuf), recvbuflen);
            printf("Answer is :" + *recvbuf);
            printf("Bytes received: %d\n", iResult);
        }
        else if (iResult == 0)
            printf("Connection closed\n");
        else
            printf("recv failed with error: %d\n", WSAGetLastError());

    } while (iResult > 0);

    // cleanup
    closesocket(ConnectSocket);
    WSACleanup();
    int k;
    std::cin >> k;
    return 0;
}