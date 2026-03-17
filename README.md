<p align="center">
   <img width="1536" height="425" alt="SecretOfTheLittleForest" src="https://github.com/user-attachments/assets/f50c6e99-54ac-408e-ab99-6e3d47e2ebce" />
</p>

# TheSecretOfTheLittleForest

### 다양한 종류의 방을 지나가면서 스테이지를 클리어해 나가는 로그라이크 형식의 체스 게임

---

## 프로젝트 소개

* 개발 인원 : 개발자 1인, 기획자 1인 (협업 프로젝트)
* 개발 기간 : 2025.09.12 ~ 2025.10.01 (3주)
* 빌드 : Android
* 개발 환경 : Unity 6.x

---

### 게임 설명

* 기본 프리셋에서 시작해 다양한 방들을 진행해나가면서 원하는 덱으로 성장시키는 게임입니다.
* 상점 방에서는 유닛을 사고 팔수 있고 전투 방에서는 보유한 덱으로 다양한 적들과 체스형식으로 대결합니다.
* 각 층마다 보스방을 클리어하면 다음 층으로 진행할 수 있고, 총 3층의 스테이지를 클리어하면 게임을 승리합니다.

### 장르

* 전략
* 로그라이크
* 체스

---

## 게임 스크린샷

### 프리셋 선택 화면

<p align="center">
  <img width="575" height="270" alt="프리셋 선택" src="https://github.com/user-attachments/assets/6322916e-1035-45e3-851e-20c2f1208526" />
</p>

### 방 선택 화면

<p align="center">
  <img width="575" height="270" alt="방 선택" src="https://github.com/user-attachments/assets/c6e330cb-83de-412a-813f-a9a439366dce" />
</p>

### 배치 변경 화면 

<p align="center">
  <img width="575" height="270" alt="배치 변경" src="https://github.com/user-attachments/assets/ed6b323a-4da5-405a-a37d-2c9f36a988a8" />
</p>

### 전투 화면

<p align="center">
  <img width="575" height="270" alt="전투 화면" src="https://github.com/user-attachments/assets/ba411e1e-8a4b-4d17-b54f-7b07591a78a9" />
</p>

---

## 프로젝트 폴더 구조

```text
Assets/                             # 프로젝트 전체 에셋 루트
└─ Scripts/                         # 게임 로직 스크립트 모음
  ├─ (루트 공용 스크립트)            # 공통 유틸리티 및 전역 설정
  ├─ DataTable/                     # 데이터 테이블 관련 스크립트
  ├─ DeckSetting/                   # 덱 편성 및 설정 관련 기능
  ├─ Game/                          # 실제 인게임 전투/플레이 관련 로직
  │  ├─ GameCanvas/                 # 인게임 캔버스 처리
  │  ├─ Node/                       # 맵 노드 및 타일 관련 기능
  │  ├─ ResultWindow/               # 전투 결과창 UI 및 처리
  │  ├─ Toy/                        # 유닛/장난감 오브젝트 관련 로직
  │  └─ Turn/                       # 턴 진행 및 턴 제어 로직
  ├─ Lobby/                         # 로비 화면 관련 기능
  ├─ OtherManagers/                 # 매니저 스크립트 모음
  ├─ Resource/                      # 리소스 로드 기능
  ├─ Shop/                          # 상점 시스템 관련 기능
  ├─ StageChoosing/                 # 스테이지 선택 화면 및 로직
  └─ Tutorial/                      # 튜토리얼 진행 관련 기능
```

---

## 주요 기능 및 시스템

###  체스 기본 시스템

| 구현 기능 | 관련 링크 |
| --- | --- |
| 체스 움직임 구현 | [ChessLogic](Assets/Scripts/Game/PlayLogic.cs) |
| 턴제 기반 시스템 구현 | [Turn](Assets/Scripts/Game/Turn) |
| 우선순위 기반 적 움직임 구현 | [EnemyTurn](Assets/Scripts/Game/Turn/EnemyTurn.cs) |

###  덱 세팅 시스템

| 구현 기능 | 관련 링크 |
| --- | --- |
| 덱 클래스 | [Deck](Assets/Scripts/DeckSetting/Deck.cs) |
| 덱 배치 조작 구현 | [ObjectControl](Assets/Scripts/Game/SetObjectControl.cs) |
| 프리셋 덱 세팅 관련 구현 | [DeckPreset](Assets/Scripts/DeckSetting/DeckSettingManager.cs) |

###  방(스테이지) 시스템

| 구현 기능 | 관련 링크 |
| --- | --- |
| 방 진행 방식 구현 | [StageChoosing](Assets/Scripts/StageChoosing) |
| 상점 방 구현 | [Shop](Assets/Scripts/Shop) |

### 그 외 기능

| 구현 기능 | 관련 링크 |
| --- | --- |
| Resource 폴더 기반 로드 방식 구현 | [Resource](Assets/Scripts/Resource) |
| CSV Data 로드 구현 | [DataTable](Assets/Scripts/DataTable) |

---

## 기술 스택 / 개발 환경

프로젝트 개발 환경 및 사용 기술

| Category        | Technology           |
| --------------- | -------------------- |
| Engine          | Unity                |
| Language        | C#                   |
| IDE             | Visual Studio        |
| Version Control | Git / GitHub         |
