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
    Sleep(5000);
    WSADATA wsaData;
    SOCKET ConnectSocket = INVALID_SOCKET;
    struct addrinfo* result = NULL,
        * ptr = NULL,
        hints;
    char mes[1024];
    int T = 22;
    int TsA = 10;
    int TeA = 4;
    int TsH = 5;
    int TeH = 12;
    int TH = 23;
    int TL = 70;
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
    std::string settings = "";
    std::string prover = "";
    std::string line;
    std::ifstream in("test.txt"); // окрываем файл для чтения
    if (in.is_open())
    {
        getline(in, line);
        
            //std::cout << line << endl;
            prover = line;
        
    }
    in.close();
    std::ifstream iny("settings.txt"); // окрываем файл для чтения
    if (iny.is_open())
    {
        getline(iny, line);

        //std::cout << line << endl;
        settings = line;

    }
    iny.close();
    cout << settings << endl;
    std::cout << prover << endl;
    std::string take_settings;

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
            //printf(recvbuf);
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
        f << guid << endl;
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
    if(settings == "")
    {
        /*      take_settings = prover + "|get_settings|";
        iResult = send(ConnectSocket, take_settings.c_str(), (int)strlen(take_settings.c_str()), 0);
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
           // take_settings = recvbuf;
            printf("Bytes received: %d\n", iResult);

        }
        else if (iResult == 0)
            printf("Connection closed\n");
        else
            printf("recv failed with error: %d\n", WSAGetLastError());
*/
        std::string asd = "23|27|17|10|20|30|10";
        //recvbuf = asd;
        ofstream fs;
        fs.open("settings.txt", ios::app);
        fs << asd;
        fs.close();
        std::ifstream insa("settings.txt");
        if (insa.is_open())
        {
            while (getline(insa, line))
            {
                //std::cout << line << endl;
                settings = line;
            }
        }
        insa.close();
    }
    //string asdasd = "12";

    //T = std::stoi(asdasd);
    string h;
    cout << T << endl;
    int count = 0;
    int count2 = 0;
    cout << settings.size() << endl;
   for (int i = 0; i < settings.size(); i++)
    {
        if (settings[i] == '|')
        {
            count++;
        }
        else
        {
            h += settings[i];
            count2++;
            if (count2 == 2 && count == 0)
            {
                T = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 1)
            {
                TsA = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 2)
            {
                TeA = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 3)
            {
                TsH = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 4)
            {
                TeH = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 5)
            {
                TH = std::stoi(h);
                count2 = 0;
                h = "";
            }
            if (count2 == 2 && count == 6)
            {
                TL = std::stoi(h);
                count2 = 0;
                h = "";
            }
        }
    }

   cout << T << " " << TsA << " " << TeA << " " << TsH << " " << TeH << " " << TH << " " << TL << endl;




    T = rand() % 13 + 10;
    bool Flag = false;
    bool Flag2 = false;

    for (int i = 0; i < 20; i++)
    {
       // Sleep(10000);
        command = prover + "|send_data|";
        
        if (T >= TsA && T<TH)
        {
            Flag = true;
            std::cout << "Начало проветривания " << T << endl;
            command += std::to_string(T);
            T = T- 3;
            goto asd;
        }
        if (T < TsA && Flag == true && T> TeA)
        {
            std::cout << "Продолжается проветривание " << T << endl;
            command += std::to_string(T);
            T = T - 4;
            goto asd;
        }
        if (T <= TeA && Flag == true)
        {
            std::cout << "Проветривание закончено " << T << endl;
            Flag = false;
            command += std::to_string(T);
            T = T + 2;
            goto asd;
        }

        if (T <= TsH && T> TL)
        {
            std::cout << "Начало подогрева " << T << endl;
            Flag2 = true;
            command += std::to_string(T);
            T = rand() % 10 + T;
            goto asd;
        }
        if (T > TsH && Flag2 == true && T<TeH)
        {
            std::cout << "Продолжается подогрев " << T << endl;
            command += std::to_string(T);
            T = rand() % 10 + T;
            
            goto asd;
        }
        if (T >= TeH && Flag2 == true)
        {
            Flag2 = false;
            command += std::to_string(T);
            std::cout << "Подогрев закончен " << T << endl;
            T = rand() % 10 + T;
            goto asd;
        }
        if (T >= TH)
        {
            std::cout << "Предельно большая температура, отправка смс " << T << endl;
            command += std::to_string(T);
            T = rand() % 5 + 20;
            goto asd;
        }
        if (T <= TL)
        {
            std::cout << "Предельно низкая теспература, отправка смс " << T << endl;
            command += std::to_string(T);
            T = rand() % 5 + 10;
            goto asd;
        }
        if (T < 27 && T>10)
        {
            std::cout << "Нормальная температура " << T << endl;
            command += std::to_string(T);
             T = T + 3;
        }
    asd:
        std::cout << command << " Command to send" << endl;
        iResult = send(ConnectSocket, command.c_str(), (int)strlen(command.c_str()), 0);
        if (iResult == SOCKET_ERROR) {
            printf("send failed with error: %d\n", WSAGetLastError());
            closesocket(ConnectSocket);
            WSACleanup();
            return 1;
        }

        printf("Bytes Sent: %ld\n", iResult);
       
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