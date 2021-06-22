#define WIN32_LEAN_AND_MEAN
#include <iostream>
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include <cstdlib>
#include<fstream>
#include "objbase.h"
using namespace std;
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
    WCHAR guid[36];
    std::string sendbuf;
    char recvbuf[DEFAULT_BUFLEN];
    int iResult;
    int recvbuflen = DEFAULT_BUFLEN;
    std::string command;

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
    //ЗАПРОС ГУИД

    std::string prover = "";
    std::string line;
    std::ifstream in("test.txt"); // окрываем файл для чтения
    if (in.is_open())
    {
        while (getline(in, line))
        {
            std::cout << line << endl;
            prover = line;
        }
    }
    in.close();
    std::cout << prover << endl;
    
    if (prover == "")
    {
        //запрос guid
        sendbuf = "|get_guid|";
        iResult = send(ConnectSocket, sendbuf.c_str(), (int)strlen(sendbuf.c_str()), 0);
        if (iResult == SOCKET_ERROR) {
            printf("send failed with error: %d\n", WSAGetLastError());
            closesocket(ConnectSocket);
            WSACleanup();
            return 1;
        }

        iResult = recv(ConnectSocket, recvbuf, recvbuflen, 0);
        if (iResult > 0)
        {
            printf(recvbuf);
            for (int i = 0; i < 36; i++)
            {
                guid[i] = recvbuf[i];
            }
            printf("Bytes received: %d\n", iResult);
            std::wcout << "GUID = " << guid << endl;
        }
        else if (iResult == 0)
            printf("Connection closed\n");
        else
            printf("recv failed with error: %d\n", WSAGetLastError());

        wofstream f;
        f.open("test.txt");
        f << guid;
        f.close();
        std::ifstream ins("test.txt");
        if (ins.is_open())
        {
            while (getline(ins, line))
            {
                std::cout << line << endl;
                prover = line;
            }
        }
        ins.close();
    }
    T = rand() % 13 + 10;
    bool Flag = false;
    bool Flag2 = false;
    bool FlagC = false;

    for (int i = 0; i < 10; i++)
    {

        command = prover + "|send_data|";
        
        if (T >= TsA && T<TH)
        {
            Flag = true;
            std::cout << "Начало проветривания " << T << endl;
            FlagC = true;
            command += std::to_string(T);
            T = T - rand() % 20 + 1;
            goto asd;
        }
        else if (T < TsA && Flag == true && T> TeA)
        {
            std::cout << "Продолжается проветривание " << T << endl;
            FlagC = true;
            command += std::to_string(T);
            T = T - rand() % 17 + 1;
            goto asd;
        }
        else if (T <= TeA && Flag == true)
        {
            std::cout << "Проветривание закончено " << T << endl;
            Flag = false;
            FlagC = true;
            command += std::to_string(T);
            T = rand() % 10 + 1;
            goto asd;
        }

        if (T <= TsH && T> TL)
        {
            std::cout << "Начало подогрева " << T << endl;
            Flag2 = true;
            FlagC = true;
            command += std::to_string(T);
            T = rand() % 15 + T;
            goto asd;
        }
        if (T > TsH && Flag2 == true && T<TeH)
        {
            std::cout << "Продолжается подогрев " << T << endl;
            FlagC = true;
            command += std::to_string(T);
            T = rand() % 10 + T;
            
            goto asd;
        }
        if (T >= TeH && Flag2 == true)
        {
            Flag2 = false;
            FlagC = true;
            command += std::to_string(T);
            std::cout << "Подогрев закончен " << T << endl;
            T = rand() % 10 + T;
            goto asd;
        }
        if (T >= TH)
        {
            std::cout << "Предельно большая температура, отправка смс " << T << endl;
            FlagC = true;
            command += std::to_string(T);
            T = rand() % 10 + 10;
            goto asd;
        }
        if (T <= TL)
        {
            std::cout << "Предельно низкая теспература, отправка смс " << T << endl;
            FlagC = true;
            command += std::to_string(T);
            T = rand() % 15 + 15;
            goto asd;
        }
        if (T < 27 && T>10)
        {
            std::cout << "Нормальная температура " << T << endl;
            
        }
    asd:
        if (FlagC == false)
        {
        command += std::to_string(T);
        }
        FlagC = false;
        
        std::cout << command << " Command to send" << endl;
        iResult = send(ConnectSocket, command.c_str(), (int)strlen(command.c_str()), 0);
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

    // cleanup
    closesocket(ConnectSocket);
    WSACleanup();
    int k;
    std::cin >> k;
    return 0;
}