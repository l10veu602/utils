퍼포스 파일 타입이 text이고, utf16 인코딩이 포함되어 있는 파일은 Windows에서 깨질 수 있습니다.
http://answers.perforce.com/articles/KB/3488

깨진 파일을 고치려면 다음과 같이 할 수 있습니다.
1. p4 client의 Line End를 unix로 바꾼다.
2. 깨진 파일을 force sync한다.
3. 깨진 파일의 파일 타입을 utf16으로 바꾸고 submit한다.
4. p4 client의 Line End를 원래대로 돌린다.

위의 방법대로 하려면 깨진 파일들을 알고 있어야 합니다.
이 프로그램은 인코딩이 utf16이고, 퍼포스 파일 타입이 text인 파일들을 찾아 파일 타입을 utf16으로 바꿔줍니다.

다음과 같이 사용할 수 있습니다.
1. p4 client의 Line End를 unix로 바꾸고 workspace를 force sync합니다.
2. 프로그램을 실행시킵니다.
3. 실행 후 변경된 파일들이 changelist에 포함되는데, 확인 후 submit 해줍니다.
4. p4 client의 Line End를 원래대로 바꿔줍니다.

실행 방법
<실행파일> <depot files path>
ex) <실행파일> //...