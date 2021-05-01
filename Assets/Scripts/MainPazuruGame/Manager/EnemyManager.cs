using System.Collections;
using UnityEngine;

/// <summary>
/// �G�̊Ǘ��N���X
/// </summary>

public class EnemyManager : MonoBehaviour
{
    //�G�̐�����p�N���X�i��������G�𐶐����A���^�[����BaseEnemy�ɖ߂��j  
    [SerializeField]
    private EnemyFactory _EnemyFactroy;

    //�G�̃X�e�[�^�X�i�G�f�[�^���i�[���ꂽ�\���̂����ꂼ��̃N���X�E�ϐ��ɑ������j
    [SerializeField]
    private Enemy_Status _EnemyStatus;

    //�G�̍U���p�^�[�����Ǘ�����N���X�ւ̃A�N�Z�X
    [SerializeField]
    private EnemyAttack _enemyAttack;

    //�G�N���X�ւ̃A�N�Z�X
    private Enemy _enemyConroll;
    public Enemy GetenemyConroll { get => _enemyConroll; set { _enemyConroll = value; } }

    //�G�̏o���񐔁i�S3�́A�G��|����Count��1�v���X�����j
    private int _enemyCount = 1;
    public int GetenemyCount { get => _enemyCount; }

    //�G�̑������̓G�X�e�[�^�X�̎Q�Ɨp�z��
    //private EnemyStatus[] Enemys = new EnemyStatus[enemyCount];
    private EnemyStatus[] Enemys;

    //�G�����j���Ȃ���΂Ȃ�Ȃ����Ԑ������J�E���g����R���[�`��
    private IEnumerator EnemyCountCol;
    public IEnumerator GetEnemyCountCol { get => EnemyCountCol; set { EnemyCountCol = value; } }

    [SerializeField]
    private RectTransform _EnemyTransform; //�G�̕\���ʒu�̌Œ�

    //�G���U�����邩�ۂ�
    private bool isEnemyAttackTurn = false;
    public bool GetisEnemyAttackTurn{ get => isEnemyAttackTurn; }

    private void Awake()
    {
        Enemys = new EnemyStatus[_EnemyStatus.GetEnemyRowsCount];
    }

    /// <summary>
    /// �G��HP���m�F���A0�ɂȂ�����Enemy������
    /// </summary>
    /// <returns></returns>
    public IEnumerator Is_Enemy_Check()
    {
        //�G��HP���[����������
        if (_enemyConroll.Get_NowHpPoint <= 0)
        {
            _enemyConroll.GetEnemyAttackCount = 0;
            GameManager.Instance.GetParticle.EnemyDestory(GameManager.Instance.GetParticle.GetParticleTransform);

            //�^�C�����~�b�g���~
            StopCoroutine(EnemyCountCol);
            EnemyCountCol = null;
            PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyDestroy);
            yield return new WaitForSeconds(2.0f);
            //�G�̍폜
            Destroy(_enemyConroll.gameObject);
            yield return new WaitForSeconds(1.0f);

            //�G�̏o���J�E���g���m�F���AEnemyCount�ȉ��������玟�̓G���o��������
            if (_enemyCount < _EnemyStatus.GetEnemyRowsCount -1)
            {
                _enemyCount++;
                GameManager.Instance.GetParticle.EnemyPop(GameManager.Instance.GetParticle.GetParticleTransform);

                GameManager.Instance.GetUIManager.SignalPopup("Stage"+ _enemyCount.ToString() + "/" + _EnemyStatus.GetEnemyRowsCount.ToString(), GameManager.Instance.GetUIManager.GetrectTransform);
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);
                yield return new WaitForSeconds(3.0f);

                //�G�̍\���̂�����
                Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);
                //�G�̐���
                InitEnemys(_enemyCount);
                yield return null;
                //�G�̃^�C���J�E���g���J�n����
                yield return null;
                StartCoroutine(EnemyCountCol);
                //yield return EnemyAttack();
            }//�o���J�E���g���z������N���A��ʂ�
            else { yield return GameManager.Instance.GetUIManager.Clear(); }
           
        }
        else if (_enemyConroll.GetEnemyTimeOver)
        {
            //�b���J�E���g�ȓ��ɓ|���Ȃ�������GameOver
            yield return GameManager.Instance.GetUIManager.GameOver();
        }
    }

    /// <summary>
    /// �G�J�E���g��0�ɂȂ�����G����̍U���������s��
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyAttack()
    {
        if (_enemyConroll.GetHindrancePiece == _enemyConroll.GetEnemyAttackCount)
        {
            _enemyAttack.EnemyAttackType((Enemy_Attack_Type)_enemyConroll.GetAttackEnemyType,GameManager.Instance.GetPieceManager.GetPieces);
            GameManager.Instance.GetParticle.EnemyAll(GameManager.Instance.GetParticle.GetParticleTransform);
            yield return new WaitForSeconds(3.0f);
            _enemyConroll.GetEnemyAttackCount = 0;
            
        }
    }

    /// <summary>
    /// �G�̏���������
    /// </summary>
    public void InitEnemys(int num)
    {
        //�G�����N���X�𐶐�����
        _EnemyFactroy = Instantiate(_EnemyFactroy);

        //���̓f�[�^�Ɖ��̖ڂ̓G�Ȃ̂��𔻒肵�A�Y���̓G�̍\���̂��󂯓n��
        Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);

        //�󂯓n�����\���̂�G�N���X�ɑ������
        _enemyConroll = _EnemyFactroy.EnemyInstance(Enemys[num]);
        _enemyConroll.transform.SetParent(_EnemyTransform);

        Instantiate(_EnemyStatus);
        EnemyCountCol = _enemyConroll.TimeCount(_enemyConroll.GetEnemyTimeLimit);
        StartCoroutine(EnemyCountCol);

    }

}